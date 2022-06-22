using Microsoft.EntityFrameworkCore;
using RepositorioAcademico.WebApp.Models;

namespace RepositorioAcademico.WebApp.Data;
public class RepositorioContext : DbContext
{
    public RepositorioContext(DbContextOptions<RepositorioContext> options) : base(options)
    {
        
    }
    
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Documento> Documentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositorioContext).Assembly);
    }
}

 