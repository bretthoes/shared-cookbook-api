using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Data.Dtos;

public class DetailedRecipeDto
{
    public required int RecipeId { get; set; }

    public required int CookbookId { get; set; }

    public required string Title { get; set; }

    public int? PersonId { get; set; }

    public string? Summary { get; set; }

    public string? ImagePath { get; set; }

    public string? VideoPath { get; set; }

    public int? PreparationTimeInMinutes { get; set; }

    public int? CookingTimeInMinutes { get; set; }

    public int? BakingTimeInMinutes { get; set; }

    public int? Servings { get; set; }

    public virtual Person? Author { get; set; }

    public virtual RecipeNutrition? Nutrition { get; set; }

    public virtual ICollection<IngredientCategory> IngredientCategories { get; set; } = [];

    public virtual ICollection<RecipeComment> RecipeComments { get; set; } = [];

    public virtual ICollection<RecipeDirection> RecipeDirections { get; set; } = [];

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];

    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = [];
}
