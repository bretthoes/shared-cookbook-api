namespace SharedCookbookApi.Data.Entities;

public class RecipeRating
{
    public required int RecipeRatingId { get; set; }

    public required int RecipeId { get; set; }

    public required int PersonId { get; set; }

    public required int RatingValue { get; set; }

    public required DateTime Created { get; set; }

    public required virtual Person Person { get; set; }

    public required virtual Recipe Recipe { get; set; }
}
