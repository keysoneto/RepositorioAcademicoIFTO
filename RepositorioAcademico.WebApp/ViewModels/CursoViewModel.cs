using System.ComponentModel.DataAnnotations;

namespace RepositorioAcademico.WebApp.ViewModels
{
    public class CursoViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O {0} precisa ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public bool Ativo { get; set; }
        [Display(Name = "Logo do Curso")]
        public IFormFile Logo { get; set; }
    }
}
