using AutoMapper;

namespace WebAPI.Data
{
    public class EfBookRepo : IBookRepo
    {
        private readonly DataDbContext _ctx;

        public EfBookRepo(DataDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<BookModel> GetAllBooks()
        {
            return _ctx.Books.ToList();
        }

        public BookModel GetOneBook(Guid id)
        {
            return _ctx.Books.FirstOrDefault(x=>x.Id == id);
        }


        public void AddMultipleBook(List<BookModel> bookDTOs)
        {
            bookDTOs.ForEach(b => b.Id = Guid.NewGuid());
            _ctx.Books.AddRange(bookDTOs);
            _ctx.SaveChanges();
        }

        public void AddBook(BookModel md)
        {
            _ctx.Books.Add(md);
        }

        public void DeleteBook(BookModel md)
        {
            _ctx.Books.Remove(md);
        }

        public void UpdateBook(BookModel md)
        {

        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
    }
}
