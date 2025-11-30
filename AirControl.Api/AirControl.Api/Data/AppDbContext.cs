using Microsoft.EntityFrameworkCore;
using AirControl.Api.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AirControl.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Tecnico> Tecnicos => Set<Tecnico>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<Foto> Fotos => Set<Foto>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<PmocRegistro> PmocRegistros => Set<PmocRegistro>();

    // 👉 AQUI é o que faltava
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Não transformar o PendingModelChangesWarning em exceção
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Cliente>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            e.Property(p => p.Endereco).HasMaxLength(200);
            e.Property(p => p.Telefone).HasMaxLength(20);
            e.Property(p => p.Email).HasMaxLength(100);
        });

        mb.Entity<Tecnico>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            e.Property(p => p.Email).HasMaxLength(100);
            e.Property(p => p.Telefone).HasMaxLength(20);
            e.Property(p => p.Cargo).IsRequired().HasMaxLength(20);

            // ÍNDICE ÚNICO SEM FILTER COM COLCHETE
            e.HasIndex(p => p.Email)
                .IsUnique()
                .HasDatabaseName("IX_Tecnicos_Email");
            // Se quiser um filtro, teria que ser jeitão PostgreSQL: "\"Email\" IS NOT NULL"
        });

        mb.Entity<OrdemServico>(e =>
        {
            e.Property(p => p.Status).IsRequired().HasMaxLength(20);
            e.Property(p => p.Prioridade).IsRequired().HasMaxLength(20);
            e.Property(p => p.Endereco).HasMaxLength(300);

            e.HasOne(p => p.Cliente)
                .WithMany(c => c.Ordens)
                .HasForeignKey(p => p.ClienteId);

            e.HasOne(p => p.Tecnico)
                .WithMany(t => t.Ordens)
                .HasForeignKey(p => p.TecnicoId);
        });

        mb.Entity<Foto>(e =>
        {
            e.Property(p => p.Tipo).IsRequired().HasMaxLength(10);
            e.Property(p => p.ContentType).HasMaxLength(50);
            e.Property(p => p.NomeArquivo).HasMaxLength(200);

            e.HasOne(p => p.OrdemServico)
                .WithMany(o => o.Fotos)
                .HasForeignKey(p => p.OrdemServicoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
