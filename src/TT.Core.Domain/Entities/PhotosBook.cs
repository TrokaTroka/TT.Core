namespace TT.Core.Domain.Entities;

public class PhotosBook : EntityBase
{
    public PhotosBook(string path, string name, Guid idBook)
    {
        Path = path;
        Name = name;
        IdBook = idBook;
    }

    public string Path { get; private set; }
    public string Name { get; private set; }
    public Guid IdBook { get; private set; }

    public Book Book { get; set; }

    public PhotosBook()
    { }
}
