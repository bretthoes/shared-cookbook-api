namespace shared_cookbook_api.Data.Dtos;

public class CookbookDto
{
    public required int CookbookId { get; set; }

    public int? CreatorPersonId { get; set; }

    public required string Title { get; set; }

    public string? ImagePath { get; set; }
}
