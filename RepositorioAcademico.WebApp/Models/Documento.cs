namespace RepositorioAcademico.WebApp.Models;

public class Documento
{
    public Documento()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int Ano { get; set; }
    public string Orientador { get; set; }
    public string URI { get; set; }
    public bool Visivel { get; set; }
    public Guid CursoId { get; set; }
    public virtual Curso Curso { get; set; }
}