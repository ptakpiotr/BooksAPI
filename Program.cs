using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Refit;
using StackExchange.Redis;
using WebAPI.Polly;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration Configuration = builder.Configuration;
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

services.AddRefitClient<IRefitClient>().ConfigureHttpClient((opts) =>{
    opts.BaseAddress = new Uri("https://api.mockaroo.com");
});

services.AddControllers();

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

services.AddScoped<IEmailSender, FluentEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
