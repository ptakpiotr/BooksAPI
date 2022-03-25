using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IRefitClient _client;
        private readonly IConfiguration _configuration;

        public BooksController(IRefitClient client,IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _client.GetBooks(_configuration["ApiKey"],10));
        }
    }
}
