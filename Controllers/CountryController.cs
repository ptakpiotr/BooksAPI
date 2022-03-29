using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryRepo _repo;
        private readonly IMapper _mapper;

        public CountryController(ILogger<CountryController> logger, ICountryRepo repo, IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllCountries()
        {
            List<CountryModel> countries = _repo.GetCountry();
            List<CountryReadDTO> output = _mapper.Map<List<CountryReadDTO>>(countries);

            return Ok(output);
        }

        [HttpGet("{id}", Name = "GetCountry")]
        public IActionResult GetCountry(int id)
        {
            CountryModel country = _repo.GetCountryById(id);

            if (country is null)
            {
                return NotFound();
            }

            CountryReadDTO output = _mapper.Map<CountryReadDTO>(country);

            return Ok(output);
        }

        [HttpPost]
        public IActionResult AddCountry(CountryAddDTO dto)
        {
            if (ModelState.IsValid)
            {
                CountryModel md = _mapper.Map<CountryModel>(dto);

                _repo.AddCountry(md);
                _repo.SaveChanges();

                return CreatedAtRoute("GetCountry",new {id = md.Id},md);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            CountryModel cm = _repo.GetCountryById(id);

            if(cm is null)
            {
                return NotFound();
            }

            _repo.DeleteCountry(cm);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult EditCountry(int id,CountryAddDTO dto)
        {
            CountryModel cm = _repo.GetCountryById(id);

            if(cm is null)
            {
                return NotFound();
            }

            _mapper.Map(dto, cm);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}
