using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositorioAcademico.WebApp.Data;
using RepositorioAcademico.WebApp.Models;

namespace RepositorioAcademico.WebApp.ViewComponents
{
    [ViewComponent(Name = "listacursos")]
    public class ListaCursosViewComponent : ViewComponent
    {
        private readonly RepositorioContext _context;
        public IEnumerable<Curso> CursosDisponiveis { get; set; }
        public ListaCursosViewComponent(RepositorioContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CursosDisponiveis = await _context.Cursos.AsNoTracking()
                .Include(e => e.Documentos)
                .Where(e => e.Documentos.Count > 0 && e.Ativo).ToListAsync();

            return View(CursosDisponiveis);
        }
    }
}
