namespace SharedCookbookApi.Data.Entities;

public class RecipeDirection
{
    public required int RecipeDirectionId { get; set; }

    public required int RecipeId { get; set; }

    public required string DirectionText { get; set; }

    public required int Ordinal { get; set; }

    public string? ImagePath { get; set; }

    public required virtual Recipe Recipe { get; set; }
}
