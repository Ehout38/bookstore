using Books.Data;
using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers;

public class BooksController(BookContext db, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var books = await db.Books.AsNoTracking().Include(book => book.Images).OrderBy(book => book.Id).ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await FindBook(id);
        return book == null ? NotFound() : View(new BookDetailsViewModel { Book = book });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int id, [Bind(Prefix = "Comment")] CommentInput input)
    {
        var book = await FindBook(id);
        if (book == null) return NotFound();
        if (!ModelState.IsValid)
            return View("Details", new BookDetailsViewModel { Book = book, Comment = input });

        db.Comments.Add(new Comment
        {
            BookId = id,
            Name = input.Name.Trim(),
            Text = input.Text.Trim()
        });
        await db.SaveChangesAsync();
        TempData["Success"] = "Your comment has been added.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(16_000_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 16_000_000)]
    public async Task<IActionResult> UploadImages(int id, List<IFormFile> images)
    {
        var book = await db.Books.FindAsync(id);
        if (book == null) return NotFound();
        if (images.Count == 0 || images.Count > 6)
        {
            TempData["Error"] = "Choose between 1 and 6 images.";
            return RedirectToAction(nameof(Details), new { id });
        }

        foreach (var file in images)
        {
            if (file.Length == 0 || file.Length > 2 * 1024 * 1024 || !await IsImage(file))
            {
                TempData["Error"] = "Use JPG, PNG or WebP images, up to 2 MB each.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        var folder = Path.Combine(environment.WebRootPath, "uploads");
        Directory.CreateDirectory(folder);
        var savedFiles = new List<string>();
        try
        {
            foreach (var file in images)
            {
                var name = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
                var path = Path.Combine(folder, name);
                savedFiles.Add(path);
                await using var stream = new FileStream(path, FileMode.CreateNew);
                await file.CopyToAsync(stream);
                db.BookImages.Add(new BookImage { BookId = id, FilePath = "/uploads/" + name });
            }
            await db.SaveChangesAsync();
        }
        catch
        {
            foreach (var path in savedFiles)
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            throw;
        }

        TempData["Success"] = $"Added {images.Count} image(s).";
        return RedirectToAction(nameof(Details), new { id });
    }

    private Task<Book?> FindBook(int id) => db.Books.AsNoTracking()
        .Include(book => book.Images)
        .Include(book => book.Comments)
        .AsSplitQuery()
        .FirstOrDefaultAsync(book => book.Id == id);

    private static async Task<bool> IsImage(IFormFile file)
    {
        var header = new byte[12];
        await using var stream = file.OpenReadStream();
        var count = await stream.ReadAtLeastAsync(header, 12, throwOnEndOfStream: false);
        if (count < 12) return false;

        return Path.GetExtension(file.FileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => header[0] == 0xff && header[1] == 0xd8 && header[2] == 0xff,
            ".png" => header.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            ".webp" => System.Text.Encoding.ASCII.GetString(header, 0, 4) == "RIFF" && System.Text.Encoding.ASCII.GetString(header, 8, 4) == "WEBP",
            _ => false
        };
    }
}
