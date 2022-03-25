using WebAPI.Data.Validation;

namespace WebAPI.Models.DTOs
{
    public class CountryAddDTO
    {
        public string Name { get; set; }

        [ValidCountryCode]
        public string CountryCode { get; set; }
    }
}
