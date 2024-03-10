namespace shared_cookbook_api.Data.Dtos;

public class PersonDto
{
    public required int PersonId { get; set; }

    public required string Email { get; set; }

    public string? Username { get; set; }

    public string? ImagePath { get; set; }
}
