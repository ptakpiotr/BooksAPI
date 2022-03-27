using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class BookModel
    {
        [Key]
        public Guid Id{ get; set; }

        public string Book { get; set; }

        public string Author { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public uint Pages { get; set; }
        public int CountryId { get; set; }
        public CountryModel Country { get; set; }
    }
}
