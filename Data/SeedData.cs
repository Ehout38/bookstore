using Books.Models;

namespace Books.Data;

public static class SeedData
{
    public static void Initialize(BookContext db)
    {
        if (db.Books.Any()) return;

        db.Books.AddRange(
            Create("The Hobbit", "J. R. R. Tolkien", "Fantasy", 420m, "A quiet hobbit joins a company of dwarves on a journey to reclaim their home. A story about courage, friendship and finding a place in a much larger world.", "hobbit"),
            Create("Pride and Prejudice", "Jane Austen", "Classic", 310m, "Elizabeth Bennet faces questions of family, reputation and love. Austen's novel follows the misunderstandings that shape her relationship with Mr Darcy.", "pride"),
            Create("The Time Machine", "H. G. Wells", "Science fiction", 280m, "An inventor travels into the distant future and discovers a world transformed. His journey becomes an exploration of society, progress and the passage of time.", "time"),
            Create("The Little Prince", "Antoine de Saint-Exupery", "Fiction", 350m, "A pilot stranded in the desert meets a young visitor from another planet. Their conversations explore friendship, responsibility and the things people overlook as they grow up.", "prince"),
            Create("Sherlock Holmes", "Arthur Conan Doyle", "Mystery", 390m, "A collection of detective stories featuring Sherlock Holmes and Dr Watson. Careful observation and logical thinking uncover the truth behind unusual cases.", "holmes"),
            Create("The Art of War", "Sun Tzu", "Strategy", 260m, "A short classic about strategy, preparation and understanding circumstances. Its chapters consider how information, planning and discipline influence the outcome of conflict.", "war")
        );
        db.SaveChanges();
    }

    private static Book Create(string title, string author, string genre, decimal price, string description, string cover) => new()
    {
        Title = title,
        Author = author,
        Genre = genre,
        Price = price,
        Description = description,
        Images = [new BookImage { FilePath = $"/images/{cover}.svg" }]
    };
}
