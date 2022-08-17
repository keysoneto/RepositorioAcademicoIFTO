using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositorioAcademico.WebApp.Data;
using RepositorioAcademico.WebApp.Models;
using RepositorioAcademico.WebApp.ViewModels;

namespace RepositorioAcademico.WebApp.Controllers;

public class DocumentosController : Controller
{
    private readonly RepositorioContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public DocumentosController(RepositorioContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index(Guid idCurso, int ano)
    {
        var documentos = _context.Documentos.AsNoTracking()
            .Include(e => e.Curso)
            .Where(x => x.CursoId == idCurso && x.Ano == ano && x.Visivel);

        return View(documentos);
    }

    [Authorize]
    public IActionResult IndexAdmin()
    {
        var documentos = _context.Documentos.AsNoTracking()
            .Include(e => e.Curso)
            .Where(x => x.Visivel)
            .OrderBy(x => x.Curso.Nome);

        return View(documentos);
    }

    [HttpPost]
    public IActionResult BuscarPorTitulo(Guid idCurso, int ano, string busca)
    {
        var documentos = _context.Documentos.AsNoTracking()
            .Include(e => e.Curso)
            .Where(x => x.CursoId == idCurso 
                && x.Ano == ano 
                && (x.Titulo.Contains(busca)
                    || x.Autor.Contains(busca)
                    || x.Orientador.Contains(busca)));

        if (documentos.Count() == 0) return View("Error");

        return View("Index", documentos);
    }

    [HttpGet]
    public IActionResult CadastrarDocumento()
    {
        return View();
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult CadastrarDocumento(DocumentoViewModel viewmodel)
    {
        if (!ModelState.IsValid) return View();

        var arquivo = UploadDocumento(viewmodel);
        var documento = new Documento
        {
            Titulo = viewmodel.Titulo,
            Autor = viewmodel.Autor,
            Orientador = viewmodel.Orientador,
            Ano = viewmodel.Ano,
            Visivel = viewmodel.Visivel,
            CursoId = viewmodel.CursoId,
            URI = arquivo,
        };

        _context.Documentos.Add(documento);
        _context.SaveChanges();

        return RedirectToAction("IndexAdmin");
    }

    [Authorize]
    public IActionResult EditarDocumento(Guid? idDocumento)
    {
        if (idDocumento is null) return NotFound();

        var documento = _context.Documentos.Find(idDocumento);

        if (documento is null) return NotFound();

        var viewmodel = new DocumentoViewModel
        {
            Id = documento.Id,
            Titulo = documento.Titulo,
            Autor = documento.Autor,
            Orientador = documento.Orientador,
            Ano = documento.Ano,
            Visivel = documento.Visivel,
        };

        return View(viewmodel);
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult EditarDocumento(DocumentoViewModel viewmodel)
    {
        if (!ModelState.IsValid) return View();

        var arquivo = UploadDocumento(viewmodel);
        var documento = _context.Documentos.Find(viewmodel.Id);

        documento.Titulo = viewmodel.Titulo;
        documento.Autor = viewmodel.Autor;
        documento.Orientador = viewmodel.Orientador;
        documento.Ano = viewmodel.Ano;
        documento.Visivel = viewmodel.Visivel;

        if (!string.IsNullOrEmpty(arquivo)) documento.URI = arquivo;

        _context.Documentos.Update(documento);
        _context.SaveChanges();

        return RedirectToAction("IndexAdmin");
    }

    private string UploadDocumento(DocumentoViewModel viewmodel)
    {
        var uniqueFileName = string.Empty;

        if (viewmodel.URI != null)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + viewmodel.URI.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                viewmodel.URI.CopyTo(fileStream);
            }
        }
        return uniqueFileName;
    }
}