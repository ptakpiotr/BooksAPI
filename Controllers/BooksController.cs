using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookRepo _repo;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookRepo repo, IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            List<BookModel> books = _repo.GetAllBooks();

            return Ok(books);
        }

        [HttpGet("{id}", Name = "GetOneBook")]
        public IActionResult GetOneBook(Guid id)
        {
            BookModel book = _repo.GetOneBook(id);

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult AddOneBook(BookAddDTO add)
        {
            if (ModelState.IsValid)
            {
                BookModel bm = _mapper.Map<BookModel>(add);
                _repo.AddBook(bm);
                _repo.SaveChanges();

                return CreatedAtRoute(nameof(GetOneBook), new { id = bm.Id }, bm);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            BookModel bm = _repo.GetOneBook(id);

            if(bm is null)
            {
                return NotFound();
            }

            _repo.DeleteBook(bm);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult EditBook(Guid id,BookAddDTO dto)
        {
            BookModel bm = _repo.GetOneBook(id);

            if (bm is null)
            {
                return NotFound();
            }

            _mapper.Map(dto, bm);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult EditBookPartially(Guid id,JsonPatchDocument<BookAddDTO> patchDoc)
        {
            BookModel bm = _repo.GetOneBook(id);

            if (bm is null)
            {
                return NotFound();
            }

            BookAddDTO dto = _mapper.Map<BookAddDTO>(bm);
            patchDoc.ApplyTo(dto);
            _mapper.Map(dto, bm);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}
