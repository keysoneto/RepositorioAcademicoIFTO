using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositorioAcademico.WebApp.Data;
using RepositorioAcademico.WebApp.Models;
using RepositorioAcademico.WebApp.ViewModels;

namespace RepositorioAcademico.WebApp.Controllers;

public class CursosController : Controller
{
    private readonly RepositorioContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CursosController(RepositorioContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var cursos = _context.Cursos.AsNoTracking().Where(e => e.Ativo == true);
        return View(cursos);
    }

    [HttpGet]
    public IActionResult CadastrarCurso()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CadastrarCurso(CursoViewModel viewmodel)
    {
        if (!ModelState.IsValid) return View();

        var arquivo = UploadImagem(viewmodel);
        var curso = new Curso
        {
            Nome = viewmodel.Nome,
            Ativo = viewmodel.Ativo,
            Logo = arquivo
        };

        _context.Cursos.Add(curso);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarCurso(Guid? idCurso)
    {
        if (idCurso is null) return NotFound();

        var curso = _context.Cursos.Find(idCurso);

        if (curso is null) return NotFound();

        var viewmodel = new CursoViewModel
        {
            Id = curso.Id,
            Nome = curso.Nome,
            Ativo = curso.Ativo,
        };

        return View(viewmodel);
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditarCurso(CursoViewModel viewmodel)
    {
        if (!ModelState.IsValid) return View();

        var arquivo = UploadImagem(viewmodel);
        var curso = _context.Cursos.Find(viewmodel.Id);
        
        curso.Nome = viewmodel.Nome;
        curso.Ativo = viewmodel.Ativo;

        if (!string.IsNullOrEmpty(arquivo)) curso.Logo = arquivo;

        _context.Cursos.Update(curso);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private string UploadImagem(CursoViewModel model)
    {
        var uniqueFileName = string.Empty;

        if (model.Logo != null)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/cursos");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Logo.CopyTo(fileStream);
            }
        }
        return uniqueFileName;
    }
}

