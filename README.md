# Books

ASP.NET Core MVC book catalog for project assignment 29. Uses Entity Framework Core and a local SQLite database.

- Catalog with title, author, genre and price.
- Book details with a gallery of multiple images.
- Comments saved in the database and displayed below each book.
- Multiple image upload: 1 to 6 JPG, PNG or WebP images per request, up to 2 MB each.
- File extension and signature checks, unique filenames and form validation.
- Six example books and local cover illustrations included.

## Run

Install the .NET 8 SDK. Open `Books.sln` in Visual Studio 2022 (17.8 or newer) and run, or use:

```sh
dotnet restore
dotnet run --project Books.csproj --urls http://localhost:5102
```

Open http://localhost:5102. On first startup, EF Core creates `App_Data/books.db` and inserts the example books. A separate database server is not needed.

Uploaded files are saved in `wwwroot/uploads`; their paths and book relationships are stored in the database. Comments and uploads remain after restarting the app. Local database files and user uploads are excluded from Git.
