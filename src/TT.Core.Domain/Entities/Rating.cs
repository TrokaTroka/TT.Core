namespace TT.Core.Domain.Entities;

public class Rating : EntityBase
{
    public Rating(int grade, string comment, Guid idRater, Guid idRated)
    {
        Grade = grade;
        Comment = comment;
        IdRater = idRater;
        IdRated = idRated;
    }

    public int Grade { get; private set; }
    public string Comment { get; private set; }
    public Guid IdRater { get; private set; }
    public Guid IdRated { get; private set; }

    public User RatedUser { get; set; }

    public Rating()
    { }
}
