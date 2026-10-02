using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paberzs_Praktiskais_3.Models;

namespace Paberzs_Praktiskais_3.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GamesApiController : ControllerBase
	{
		private readonly AppDbContext _context;

		public GamesApiController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("by-genre")]
		public IActionResult GetGamesByGenre([FromBody] GenreFilterDto filter)
		{
			if (filter == null || string.IsNullOrWhiteSpace(filter.Genre))
				return BadRequest("Lūdzu, ievadiet žanru.");

			var games = _context.Games
				.Include(g => g.Developer)
				.Where(g => g.Genre.ToLower() == filter.Genre.ToLower())
				.Select(g => new { g.Id, g.Title, g.Genre, Developer = g.Developer.Name, g.Price })
				.ToList();

			return Ok(games);
		}

		[HttpPost("devs-by-country")]
		public IActionResult GetDevsByCountry([FromBody] CountryFilterDto filter)
		{
			if (filter == null || string.IsNullOrWhiteSpace(filter.Country))
				return BadRequest("Lūdzu, ievadiet valsti.");

			var devs = _context.Developers
				.Where(d => d.Country.ToLower() == filter.Country.ToLower())
				.Select(d => new { d.Id, d.Name, d.Country })
				.ToList();

			return Ok(devs);
		}

		[HttpPost("by-max-price")]
		public IActionResult GetGamesByPrice([FromBody] PriceFilterDto filter)
		{
			var games = _context.Games
				.Where(g => g.Price <= filter.MaxPrice)
				.OrderBy(g => g.Price)
				.Select(g => new { g.Id, g.Title, g.Price, g.Genre })
				.ToList();

			return Ok(games);
		}

		[HttpGet("top-newest")]
		public IActionResult GetTopNewestGames()
		{
			var newest = _context.Games
				.OrderByDescending(g => g.ReleaseYear)
				.Take(3)
				.Select(g => new { g.Id, g.Title, g.ReleaseYear, g.Genre })
				.ToList();

			return Ok(newest);
		}

		[HttpGet("games-per-developer")]
		public IActionResult GetGamesPerDeveloper()
		{
			var stats = _context.Developers
				.Select(d => new
				{
					Developer = d.Name,
					TotalGames = d.Games.Count()
				})
				.OrderByDescending(x => x.TotalGames)
				.ToList();

			return Ok(stats);
		}

		[HttpGet("all-overview")]
		public IActionResult GetAllOverview()
		{
			var games = _context.Games
				.Include(g => g.Developer)
				.Include(g => g.Platform)
				.Select(g => new
				{
					g.Id,
					g.Title,
					Platform = g.Platform.Name,
					Developer = g.Developer.Name,
					g.ReleaseYear,
					g.Price
				})
				.ToList();

			return Ok(games);
		}

		[HttpDelete("delete/{id}")]
		public IActionResult DeleteGame(int id)
		{
			var game = _context.Games.FirstOrDefault(g => g.Id == id);
			if (game == null)
				return NotFound($"Spēle ar ID {id} netika atrasta.");

			_context.Games.Remove(game);
			_context.SaveChanges();

			return Ok(new { message = $"Spēle '{game.Title}' (ID: {id}) tika veiksmīgi izdzēsta!" });
		}
	}


	public class GenreFilterDto
	{
		public string Genre { get; set; }
	}

	public class CountryFilterDto
	{
		public string Country { get; set; }
	}

	public class PriceFilterDto
	{
		public decimal MaxPrice { get; set; }
	}
}