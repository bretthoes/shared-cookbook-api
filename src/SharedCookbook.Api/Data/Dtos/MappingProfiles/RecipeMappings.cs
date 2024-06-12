using AutoMapper;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Data.Dtos.MappingProfiles;

public class RecipeMappings : Profile
{
    public RecipeMappings()
    {
        CreateMap<DetailedRecipeDto, Recipe>().ReverseMap();

        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.AuthorName, opt => opt
                .MapFrom(src => src.Author != null ? src.Author.DisplayName : null))
            .ForMember(dest => dest.AuthorImagePath, opt => opt
                .MapFrom(src => src.Author != null ? src.Author.ImagePath : null))
            .ForMember(dest => dest.AverageRating, opt => opt
                .MapFrom(src => src.RecipeRatings
                    .Any() ? (int)src.RecipeRatings
                        .Average(r => r.RatingValue) : 0))
            .ForMember(dest => dest.TotalRatings, opt => opt
                .MapFrom(src => src.RecipeRatings.Count));

    }
}
