﻿namespace SharedCookbook.Api.Data.Entities;

public class RecipeRating
{
    public required int RecipeRatingId { get; set; }

    public required int RecipeId { get; set; }

    public required int PersonId { get; set; }

    public required int RatingValue { get; set; }

    public required DateTime Created { get; set; }
}
