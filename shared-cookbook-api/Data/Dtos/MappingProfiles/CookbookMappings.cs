using AutoMapper;
using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Data.Dtos.MappingProfiles;

public class CookbookMappings : Profile
{
    public CookbookMappings()
    {
        CreateMap<CookbookDto, Cookbook>().ReverseMap();
    }
}
