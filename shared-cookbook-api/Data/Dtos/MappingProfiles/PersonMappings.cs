using AutoMapper;
using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Data.Dtos.MappingProfiles
{
    public class PersonMappings : Profile
    {
        public PersonMappings()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
        }
    }
}
