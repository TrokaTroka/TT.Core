namespace TT.Core.Application.Dtos.ViewModels;

public class BookshellDto
{
    public BookshellDto(Guid id, string title, string owner, double rate, string list, bool favorite)
    {
        Id = id;
        Title = title;
        Owner = owner;
        Rate = rate;
        Path = list;
        Favorite = favorite;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Owner { get; set; }
    public double Rate { get; set; }
    public string Path { get; set; }
    public bool Favorite { get; set; }

    public BookshellDto()
    {

    }
}
