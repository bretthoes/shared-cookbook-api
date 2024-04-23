using AutoMapper;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Data.Dtos.MappingProfiles;

public class CookbookMappings : Profile
{
    public CookbookMappings()
    {
        CreateMap<CookbookDto, Cookbook>().ReverseMap();
        CreateMap<CreateCookbookDto, Cookbook>().ReverseMap();
    }
}
