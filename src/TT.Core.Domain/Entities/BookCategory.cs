namespace TT.Core.Domain.Entities;

public class BookCategory : EntityBase
{ 
    public BookCategory(Guid idBook, Guid idCategory)
    {
        IdBook = idBook;
        IdCategory = idCategory;
    }

    public Guid IdBook { get; private set; }
    public Guid IdCategory { get; private set; }

    public Book Book { get; set; }
    public Category Category { get; set; }
    public BookCategory()
    { }
}
