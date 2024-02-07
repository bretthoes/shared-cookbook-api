namespace SharedCookbookApi.Data.Entities;

public class IngredientCategory
{
    public required int IngredientCategoryId { get; set; }

    public required string Title { get; set; }

    public required int RecipeId { get; set; }

    public required virtual Recipe Recipe { get; set; }
}
