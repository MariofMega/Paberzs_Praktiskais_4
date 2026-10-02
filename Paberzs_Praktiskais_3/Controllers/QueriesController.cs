using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paberzs_Praktiskais_3.Models;

namespace Paberzs_Praktiskais_3.Controllers
{
    public class QueriesController : Controller
    {
        private readonly AppDbContext _context;

        public QueriesController(AppDbContext context)
        {
            _context = context;
        }

    
        public IActionResult Index() => View();

        public IActionResult GamesByGenre(string genre)
        {
            ViewBag.SearchGenre = genre;
            var games = string.IsNullOrEmpty(genre)
                ? new List<Game>()
                : _context.Games.Include(g => g.Developer)
                                .Where(g => g.Genre.ToLower() == genre.ToLower())
                                .ToList();
            return View(games);
        }

     
        public IActionResult DevsByCountry(string country)
        {
            ViewBag.SearchCountry = country;
            var devs = string.IsNullOrEmpty(country)
                ? new List<Developer>()
                : _context.Developers.Where(d => d.Country.ToLower() == country.ToLower()).ToList();
            return View(devs);
        }

    
        public IActionResult GamesByPrice(decimal? maxPrice)
        {
            ViewBag.MaxPrice = maxPrice;
            var games = maxPrice == null
                ? new List<Game>()
                : _context.Games.Where(g => g.Price <= maxPrice.Value).OrderBy(g => g.Price).ToList();
            return View(games);
        }

        public IActionResult TopNewestGames()
        {
            var games = _context.Games.OrderByDescending(g => g.ReleaseYear).Take(3).ToList();
            return View(games);
        }

        public IActionResult GamesPerDeveloper()
        {
            var result = _context.Developers
                .Select(d => new DeveloperGameCountViewModel
                {
                    DeveloperName = d.Name,
                    TotalGames = d.Games.Count()
                })
                .OrderByDescending(x => x.TotalGames)
                .ToList();

            return View(result);
        }

        public IActionResult AllGamesOverview()
        {
            var games = _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Platform)
                .OrderBy(g => g.Title)
                .ToList();
            return View(games);
        }
    }

    public class DeveloperGameCountViewModel
    {
        public string DeveloperName { get; set; }
        public int TotalGames { get; set; }
    }
}