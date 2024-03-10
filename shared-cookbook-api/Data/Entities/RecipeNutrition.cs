using SharedCookbookApi.Data.Entities;

namespace shared_cookbook_api.Data.Entities;

public class RecipeNutrition
{
    public required int RecipeNutritionId { get; set; }

    public int? RecipeId { get; set; }

    public int? Calories {  get; set; }

    public int? Protein { get; set; }

    public int? Fat { get; set; }

    public int? Carbohydrates { get; set; }

    public int? Sugar { get; set; }

    public int? Fiber { get; set; }

    public int? Sodium { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
