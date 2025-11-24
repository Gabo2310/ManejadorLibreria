using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ManejadorLibreria.Models;

namespace ManejadorLibreria.Controllers;

public class LibrosController : Controller
{
    private readonly ILogger<LibrosController> _logger;

    public LibrosController(ILogger<LibrosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Libro { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
