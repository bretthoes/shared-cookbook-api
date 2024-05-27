namespace SharedCookbook.Api.Data.Entities;

public class RecipeIngredient
{
    public required int RecipeIngredientId { get; set; }

    public required int RecipeId { get; set; }

    public required string IngredientName { get; set; }

    public required int Ordinal { get; set; }

    public required bool Optional { get; set; }
}
