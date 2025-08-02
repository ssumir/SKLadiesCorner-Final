using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SKLadiesCorner.Models;
using SKLadiesCorner.Data; //Products
using Microsoft.EntityFrameworkCore; //Products

namespace SKLadiesCorner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; //Products

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) //Products
        {
            _logger = logger;
            _context = context; //Products
        }

        public async Task<IActionResult> Index() //Products
        {
            var products = await _context.Product.ToListAsync(); //Products
            return View(products); //Products
        }

        // Action to handle the Gallery page request
        public async Task<IActionResult> Gallery() //Gallery
        {
            var products = await _context.Product.ToListAsync(); //Gallery
            return View(products); //Gallery
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}