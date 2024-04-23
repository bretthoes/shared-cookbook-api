using AutoMapper;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Data.Dtos.MappingProfiles;

public class CookbookMappings : Profile
{
    public CookbookMappings()
    {
        CreateMap<CookbookDto, Cookbook>().ReverseMap();
        CreateMap<CreateCookbookDto, Cookbook>().ReverseMap();
    }
}
