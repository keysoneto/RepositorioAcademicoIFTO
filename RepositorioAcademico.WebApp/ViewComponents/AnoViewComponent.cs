using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositorioAcademico.WebApp.Data;
using RepositorioAcademico.WebApp.ViewModels;

namespace RepositorioAcademico.WebApp.ViewComponents
{
    [ViewComponent(Name = "Ano")]
    public class AnoViewComponent : ViewComponent
    {
        private readonly RepositorioContext _context;
        public SelectAnos AnosDisponiveis { get; set; }
        public AnoViewComponent(RepositorioContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid idCurso)
        {
            var anos = await _context.Documentos.AsNoTracking().Where(e => e.Visivel && e.CursoId == idCurso).Select(e => e.Ano).ToListAsync();
            var anosDistinct = anos.Distinct();

            AnosDisponiveis = new SelectAnos
            {
                CursoId = idCurso,
                AnosDisponiveis = anosDistinct
            };

            return View(AnosDisponiveis);
        }
    }
}
