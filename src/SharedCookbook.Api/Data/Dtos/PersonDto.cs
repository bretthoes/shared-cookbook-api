namespace SharedCookbook.Api.Data.Dtos;

public class PersonDto
{
    public required int PersonId { get; set; }

    public required string Email { get; set; }

    public string? DisplayName { get; set; }

    public string? ImagePath { get; set; }
}
