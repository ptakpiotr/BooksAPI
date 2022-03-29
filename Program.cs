using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Refit;
using Serilog;
using StackExchange.Redis;
using System.Text;
using WebAPI.Jobs;
using WebAPI.Polly;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration Configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
builder.Host.UseSerilog();
// Add services to the container.

services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseNpgsql(Configuration.GetConnectionString("MainConn"));
}).AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.SignIn.RequireConfirmedAccount = true;
    opts.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

services.AddDbContext<DataDbContext>(opts =>
{
    opts.UseNpgsql(Configuration.GetConnectionString("DataConn"));
});

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = Configuration["Jwt:ValidIssuer"],
        ValidAudience = Configuration["Jwt:ValidAudience"],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value))
    };
});

services.AddRefitClient<IRefitClient>().ConfigureHttpClient((opts) =>{
    opts.BaseAddress = new Uri("https://api.mockaroo.com");
});
services.AddControllers().AddNewtonsoftJson((opts) =>
{
    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddStackExchangeRedisCache(opts =>
{
    RedisRetryPolicies rpc = new();
    try
    {
        rpc.ConstDelayRetry.Execute(() =>
        {
            opts.Configuration = Configuration.GetConnectionString("RedisConn");
        });
    }
    catch (RedisConnectionException ex)
    {
        //do nothing right now :)
    }
});

services.AddHangfire(opts =>
{
    RedisRetryPolicies rpc = new();
    try
    {
        rpc.ExpDelayHangRetry.Execute(() => {
            opts.UsePostgreSqlStorage(Configuration.GetConnectionString("HangConn"));
        });
    }catch(NpgsqlException ex)
    {

    }
});

services.AddScoped<IBookRepo, EfBookRepo>();
services.AddScoped<IRecurringJobs, RecurringJobs>();
services.AddScoped<IEmailSender, FluentEmailSender>();

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}
app.UseHttpsRedirection();

app.UseHangfireServer();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseRecurringJobs();
app.MapControllers();

app.Run();
