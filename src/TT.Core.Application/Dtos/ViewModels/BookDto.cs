namespace TT.Core.Application.Dtos.ViewModels;

public class BookDto
{
    public BookDto(Guid id, string title, string author, 
        string isbn, string publisher, string model, string language,
        string reason, decimal buyPrice, DateTime buyDate, string ownerName, double ownerRating, bool favorite,
        IEnumerable<string> imagePaths, IEnumerable<string> categories)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        Publisher = publisher;
        Model = model;
        Language = language;
        Reason = reason;
        BuyPrice = buyPrice;
        BuyDate = buyDate;
        OwnerName = ownerName;
        OwnerRating = ownerRating;
        Favorite = favorite;
        ImagePaths = imagePaths;
        Categories = categories;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public string Publisher { get; set; }
    public string Model { get; set; }
    public string Language { get; set; }
    public string Reason { get; set; }
    public decimal BuyPrice { get; set; }
    public DateTime BuyDate { get; set; }
    public string OwnerName { get; set; }
    public double OwnerRating { get; set; }
    public bool Favorite { get; set; }
    public IEnumerable<string> ImagePaths { get; set; }
    public IEnumerable<string> Categories { get; set; }
}
