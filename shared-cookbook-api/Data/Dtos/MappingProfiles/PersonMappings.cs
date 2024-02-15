using AutoMapper;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Services;

namespace shared_cookbook_api.Data.Dtos.MappingProfiles
{
    public class PersonMappings : Profile
    {
        public PersonMappings()
        {
            CreateMap<CreatePersonDto, Person>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();

            CreateMap<PersonDto, Person>().ReverseMap();

            CreateMap<UpdatePersonDto, Person>().ReverseMap();

            CreateMap<LoginDto, Person>().ReverseMap();
        }
    }
}
