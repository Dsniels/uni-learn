using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using uni.learn.core.Entities;
using uni.learn.core.Entity;

namespace uni.learn.BussinesLogic.Data;

public class MainDbContext : DbContext
{

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    public DbSet<Curso> Curso { get; set; }
    public DbSet<Temas> Tema { get; set; }
    public DbSet<Voto> Voto { get; set; }
    public DbSet<CursoVisto> CursoVistos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Temas>().HasIndex(t => new { t.Nombre }).IsUnique();
    }


}
