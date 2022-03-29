
namespace WebAPI.Data
{
    public interface ICountryRepo
    {
        void AddCountry(CountryModel cm);
        void DeleteCountry(CountryModel cm);
        List<CountryModel> GetCountry();
        CountryModel GetCountryById(int id);
        void SaveChanges();
    }
}