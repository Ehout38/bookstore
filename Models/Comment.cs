using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public class Comment
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    [Required, StringLength(80)]
    public string Name { get; set; } = "";
    [Required, StringLength(2000)]
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
