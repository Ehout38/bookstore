namespace Books.Models;

public class BookImage
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public string FilePath { get; set; } = "";
}
