using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<RegisterModel, IdentityUser>();
            CreateMap<LoginModel, IdentityUser>();

            CreateMap<BookAddDTO, BookModel>().ReverseMap();
            CreateMap<CountryAddDTO, CountryModel>().ReverseMap();

            CreateMap<BookModel, BookReadDTO>();
            CreateMap<CountryModel, CountryReadDTO>();
        }
    }
}
