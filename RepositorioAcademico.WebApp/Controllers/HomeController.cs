using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositorioAcademico.WebApp.Data;
using RepositorioAcademico.WebApp.Models;
using RepositorioAcademico.WebApp.ViewModels;

namespace RepositorioAcademico.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RepositorioContext _context;

    public HomeController(ILogger<HomeController> logger, RepositorioContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var documentos = _context.Documentos.AsNoTracking()
            .Include(e => e.Curso)
            .Where(x => x.Visivel)
            .OrderBy(x => x.Curso.Nome)
            .ThenBy(x => x.Titulo);

        return View(documentos);
    }

    [HttpPost]
    public IActionResult BuscarTodosPorTitulo(string titulo)
    {
        var documentos = _context.Documentos.AsNoTracking()
            .Include(e => e.Curso)
            .Where(x => x.Titulo.Contains(titulo)).ToList();

        if (documentos.Count() == 0) return View("Error");

        return View("Index", documentos);
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