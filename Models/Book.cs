using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public class Book
{
    public int Id { get; set; }
    [Required, StringLength(150)]
    public string Title { get; set; } = "";
    [Required, StringLength(100)]
    public string Author { get; set; } = "";
    [Required, StringLength(60)]
    public string Genre { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public List<Comment> Comments { get; set; } = [];
    public List<BookImage> Images { get; set; } = [];
}
