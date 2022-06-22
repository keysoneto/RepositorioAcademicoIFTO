using Microsoft.AspNetCore.Identity;

namespace RepositorioAcademico.WebApp.Models;

public class ApplicationUser : IdentityUser
{
    public string Cpf { get; set; }
}