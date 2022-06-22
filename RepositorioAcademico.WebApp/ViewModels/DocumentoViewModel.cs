namespace RepositorioAcademico.WebApp.ViewModels
{
    public class DocumentoViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        public string Orientador { get; set; }
        public IFormFile URI { get; set; }
        public bool Visivel { get; set; }
        public Guid CursoId { get; set; }
    }
}
