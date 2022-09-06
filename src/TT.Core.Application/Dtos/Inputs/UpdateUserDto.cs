using Microsoft.AspNetCore.Http;

namespace TT.Core.Application.Dtos.Inputs;

public class UpdateUserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Document { get; set; }

    public string Email { get; set; }

    public IFormFile Avatar { get; set; }
}
