namespace TT.Core.Domain.Entities;

public class Category : EntityBase
{
    public Category(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public List<BookCategory> BookCategories { get; set; }

    public Category()
    { }
}
