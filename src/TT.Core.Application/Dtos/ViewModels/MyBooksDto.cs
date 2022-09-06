namespace TT.Core.Application.Dtos.ViewModels;

public class MyBooksDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Model { get; set; }

    public string Language { get; set; }

    public decimal BuyPrice { get; set; }
}
