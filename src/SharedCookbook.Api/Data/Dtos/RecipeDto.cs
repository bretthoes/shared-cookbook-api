namespace SharedCookbook.Api.Data.Dtos;

public class RecipeDto
{
    public required int RecipeId { get; set; }

    public required int CookbookId { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorImagePath { get; set; }

    public int AverageRating { get; set; }

    public int TotalRatings { get; set; }

    public required string Title { get; set; }

    public int? PersonId { get; set; }

    public string? Summary { get; set; }

    public string? ImagePath { get; set; }

    public string? VideoPath { get; set; }

    public int? PreparationTimeInMinutes { get; set; }

    public int? CookingTimeInMinutes { get; set; }

    public int? BakingTimeInMinutes { get; set; }

    public int? Servings { get; set; }
}
