namespace SharedCookbook.Api.Data.Entities;

public class IngredientCategory
{
    public required int IngredientCategoryId { get; set; }

    public required string Title { get; set; }

    public required int RecipeId { get; set; }
}
