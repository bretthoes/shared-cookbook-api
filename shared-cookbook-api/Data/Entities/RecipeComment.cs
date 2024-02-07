namespace SharedCookbookApi.Data.Entities;

public class RecipeComment
{
    public required int RecipeCommentId { get; set; }

    public required int RecipeId { get; set; }

    public required int PersonId { get; set; }

    public required string CommentText { get; set; }

    public required DateTime Created { get; set; }

    public required virtual Person Author { get; set; }

    public required virtual Recipe Recipe { get; set; }
}
