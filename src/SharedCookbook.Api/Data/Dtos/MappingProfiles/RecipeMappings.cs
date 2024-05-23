using AutoMapper;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Data.Dtos.MappingProfiles;

public class RecipeMappings : Profile
{
    public RecipeMappings()
    {
        CreateMap<DetailedRecipeDto, Recipe>().ReverseMap();
    }
}
