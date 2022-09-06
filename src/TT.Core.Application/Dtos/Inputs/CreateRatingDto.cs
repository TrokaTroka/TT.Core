namespace TT.Core.Application.Dtos.Inputs;

public class CreateRatingDto
{
    public int Grade { get; set; }

    public string Comment { get; set; }

    public Guid IdRated { get; set; }
}
