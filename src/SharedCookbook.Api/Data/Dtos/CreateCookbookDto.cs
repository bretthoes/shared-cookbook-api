namespace SharedCookbook.Api.Data.Dtos;

public class CreateCookbookDto
{
    public required int CreatorPersonId { get; set; }

    public required string Title { get; set; }

    public required IFormFile Cover { get; set; }
}
