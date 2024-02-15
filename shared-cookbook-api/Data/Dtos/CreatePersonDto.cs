namespace shared_cookbook_api.Data.Dtos;

public class CreatePersonDto
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}
