using Books.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDirectory);
builder.Services.AddDbContext<BookContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(dataDirectory, "books.db")}"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Books}/{action=Index}/{id?}");
app.Run();
