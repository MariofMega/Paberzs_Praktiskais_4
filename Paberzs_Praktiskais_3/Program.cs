using Microsoft.EntityFrameworkCore;
using Paberzs_Praktiskais_3.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SuperGamesDB_Task2;Trusted_Connection=True;TrustServerCertificate=True;"));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    SeedData(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Queries}/{action=Index}/{id?}");

app.Run();


void SeedData(AppDbContext context)
{
    if (!context.Developers.Any())
    {
        var pc = new Platform { Name = "PC" };
        var ps5 = new Platform { Name = "PlayStation 5" };
        var xbox = new Platform { Name = "Xbox Series X" };

        var valve = new Developer { Name = "Valve", Country = "USA" };
        var cdpr = new Developer { Name = "CD Projekt Red", Country = "Poland" };
        var rockstar = new Developer { Name = "Rockstar Games", Country = "USA" };

        context.Platforms.AddRange(pc, ps5, xbox);
        context.Developers.AddRange(valve, cdpr, rockstar);

        context.Games.AddRange(
            new Game { Title = "Half-Life: Alyx", Genre = "Shooter", ReleaseYear = 2020, Price = 59.99m, Developer = valve, Platform = pc },
            new Game { Title = "Counter-Strike 2", Genre = "Shooter", ReleaseYear = 2023, Price = 0.00m, Developer = valve, Platform = pc },
            new Game { Title = "The Witcher 3", Genre = "RPG", ReleaseYear = 2015, Price = 29.99m, Developer = cdpr, Platform = ps5 },
            new Game { Title = "Cyberpunk 2077", Genre = "RPG", ReleaseYear = 2020, Price = 49.99m, Developer = cdpr, Platform = pc },
            new Game { Title = "Red Dead Redemption 2", Genre = "Action", ReleaseYear = 2018, Price = 39.99m, Developer = rockstar, Platform = ps5 }
        );

        context.SaveChanges();
    }
}