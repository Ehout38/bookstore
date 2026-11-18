namespace Books.Models;

public class BookDetailsViewModel
{
    public Book Book { get; set; } = null!;
    public CommentInput Comment { get; set; } = new();
}
