namespace WebAPI.Data
{
    public class EfCountryRepo : ICountryRepo
    {
        private readonly DataDbContext _ctx;

        public EfCountryRepo(DataDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<CountryModel> GetCountry()
        {
            return _ctx.Countries.ToList();
        }

        public CountryModel GetCountryById(int id)
        {
            CountryModel cm = _ctx.Countries.FirstOrDefault(c => c.Id == id);

            return cm;
        }

        public void AddCountry(CountryModel cm)
        {
            _ctx.Countries.Add(cm);
        }

        public void DeleteCountry(CountryModel cm)
        {
            List<BookModel> books = _ctx.Books.Where(b => b.CountryId == cm.Id).ToList();
            _ctx.Books.RemoveRange(books);

            _ctx.Countries.Remove(cm);
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
    }
}
