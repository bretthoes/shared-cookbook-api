namespace SharedCookbookApi.Data.Entities;

public class RecipeIngredient
{
    public required int RecipeIngredientId { get; set; }

    public required int RecipeId { get; set; }

    public required string IngredientName { get; set; }

    public required int Ordinal { get; set; }

    public required bool Optional { get; set; }

    public required virtual Recipe Recipe { get; set; }
}
