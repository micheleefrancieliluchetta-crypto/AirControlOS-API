using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AirControl.Api.Models;

namespace AirControl.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Tecnico> Tecnicos => Set<Tecnico>();
        public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
        public DbSet<Foto> Fotos => Set<Foto>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<PmocRegistro> PmocRegistros => Set<PmocRegistro>();

        // NOVO: tabela de empresas
        public DbSet<Empresa> Empresas => Set<Empresa>();

        // Ignora o PendingModelChangesWarning (não vira exceção)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // ==========================
            // EMPRESA
            // ==========================
            mb.Entity<Empresa>(e =>
            {
                e.ToTable("Empresas");

                e.HasKey(p => p.Id);

                e.Property(p => p.NomeFantasia)
                    .IsRequired()
                    .HasMaxLength(120);

                e.Property(p => p.RazaoSocial)
                    .HasMaxLength(150);

                e.Property(p => p.Cnpj)
                    .HasMaxLength(18);   // 00.000.000/0000-00

                e.Property(p => p.InscricaoEstadual)
                    .HasMaxLength(30);

                e.Property(p => p.Ativo)
                    .HasDefaultValue(true);

                // CNPJ único (se você quiser)
                e.HasIndex(p => p.Cnpj)
                    .IsUnique()
                    .HasDatabaseName("IX_Empresas_Cnpj");
            });

            // ==========================
            // CLIENTE
            // ==========================
            mb.Entity<Cliente>(e =>
            {
                e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                e.Property(p => p.Endereco).HasMaxLength(200);
                e.Property(p => p.Telefone).HasMaxLength(20);
                e.Property(p => p.Email).HasMaxLength(100);
            });

            // ==========================
            // TÉCNICO
            // ==========================
            mb.Entity<Tecnico>(e =>
            {
                e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                e.Property(p => p.Email).HasMaxLength(100);
                e.Property(p => p.Telefone).HasMaxLength(20);
                e.Property(p => p.Cargo).IsRequired().HasMaxLength(20);

                e.HasIndex(p => p.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Tecnicos_Email");
            });

            // ==========================
            // ORDEM DE SERVIÇO
            // ==========================
            mb.Entity<OrdemServico>(e =>
            {
                e.Property(p => p.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                e.Property(p => p.Prioridade)
                    .IsRequired()
                    .HasMaxLength(20);

                e.Property(p => p.Endereco)
                    .HasMaxLength(300);

                // relacionamento com Cliente (opcional)
                e.HasOne(p => p.Cliente)
                    .WithMany(c => c.Ordens)
                    .HasForeignKey(p => p.ClienteId);

                // relacionamento com Técnico (opcional)
                e.HasOne(p => p.Tecnico)
                    .WithMany(t => t.Ordens)
                    .HasForeignKey(p => p.TecnicoId);

                // NOVO: relacionamento opcional com Empresa
                // (por enquanto você pode deixar EmpresaId null nas antigas
                //  e usar 1 para as novas)
                e.HasOne(p => p.Empresa)
                    .WithMany(emp => emp.Ordens)
                    .HasForeignKey(p => p.EmpresaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================
            // FOTO
            // ==========================
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
}
