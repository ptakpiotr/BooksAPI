using System.ComponentModel.DataAnnotations;
using WebAPI.Data.Validation;

namespace WebAPI.Models
{
    public class CountryModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ValidCountryCode]
        public string CountryCode { get; set; }
        public ICollection<BookModel> Books{ get; set; }
    }
}
