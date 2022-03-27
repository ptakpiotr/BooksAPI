
namespace WebAPI.Data
{
    public interface IBookRepo
    {
        void AddBook(BookModel md);
        void DeleteBook(BookModel md);
        List<BookModel> GetAllBooks();
        BookModel GetOneBook(Guid id);
        void SaveChanges();
        void UpdateBook(BookModel md);
        void AddMultipleBook(List<BookModel> bookDTOs);
    }
}