namespace RepositorioAcademico.WebApp.Models;

public class Curso
{
    public Curso()
    {
        Id = Guid.NewGuid();
        Documentos = new HashSet<Documento>();
    }
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
    public string Logo { get; set; }
    public virtual ICollection<Documento> Documentos { get; set; }
}