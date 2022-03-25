using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs
{
    public class BookAddDTO
    {
        public string Book { get; set; }

        public string Author { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public uint Pages { get; set; }
        public int CountryId { get; set; }
    }
}
