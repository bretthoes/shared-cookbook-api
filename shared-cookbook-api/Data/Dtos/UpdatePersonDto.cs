namespace shared_cookbook_api.Data.Dtos;

public class UpdatePersonDto
{
    public int? PersonId { get; set; }

    public string? Email { get; set; }

    public string? DisplayName { get; set; }

    public string? ImagePath { get; set; }
}
