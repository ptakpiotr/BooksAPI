namespace WebAPI.Models.DTOs
{
    public class BookReadDTO
    {

        public Guid Id { get; set; }

        public string Book { get; set; }

        public string Author { get; set; }


        public string Email { get; set; }

        public uint Pages { get; set; }
        public int CountryId { get; set; }
        public CountryModel Country { get; set; }
    }
}
