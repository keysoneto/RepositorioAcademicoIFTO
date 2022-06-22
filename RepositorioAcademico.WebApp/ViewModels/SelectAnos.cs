namespace RepositorioAcademico.WebApp.ViewModels;

public class SelectAnos
{
    public Guid CursoId { get; set; }
    public IEnumerable<int> AnosDisponiveis { get; set; }
}

