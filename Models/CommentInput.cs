using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public class CommentInput
{
    [Required(ErrorMessage = "Enter your name.")]
    [StringLength(80, ErrorMessage = "Use no more than 80 characters.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Enter a comment.")]
    [StringLength(2000, ErrorMessage = "Use no more than 2000 characters.")]
    public string Text { get; set; } = "";
}
