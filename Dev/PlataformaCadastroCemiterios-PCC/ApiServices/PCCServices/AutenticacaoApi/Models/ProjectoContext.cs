using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;


namespace AutenticacaoApi.Models;

public partial class ProjectoContext : DbContext
{
    public ProjectoContext()
    {
    }

    public ProjectoContext(DbContextOptions<ProjectoContext> options)
        : base(options)
    {
    }
     
     

    public virtual DbSet<Funcionalidade> Funcionalidade { get; set; }

    public virtual DbSet<Grupo> Grupo { get; set; }

    public virtual DbSet<GrupoUtilizador> GrupoUtilizador { get; set; }
     
    public virtual DbSet<Permissoes> Permissoes { get; set; }
     

    public virtual DbSet<Utilizador> Utilizador { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=projecto;Username=postgres;Password=toor;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");
         
        modelBuilder.Entity<Funcionalidade>(entity =>
        {
            entity.HasKey(e => e.CodigoFuncionalidade).HasName("funcionalidades_pkey");

            entity.ToTable("funcionalidades", "acesso");

            entity.Property(e => e.CodigoFuncionalidade)
                .ValueGeneratedNever()
                .HasColumnName("codigo_funcionalidade");
            entity.Property(e => e.CodigoPai).HasColumnName("codigo_pai");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("grupos_pkey");

            entity.ToTable("grupos", "acesso");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<GrupoUtilizador>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("grupos_utilizadores_pkey");

            entity.ToTable("grupos_utilizadores", "acesso");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.GrupoId).HasColumnName("grupo_id");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");
        });
         
        modelBuilder.Entity<Permissoes>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("permissoes_pkey");

            entity.ToTable("permissoes", "acesso");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.FuncionalidadeId).HasColumnName("funcionalidade_id");
            entity.Property(e => e.GrupoId).HasColumnName("grupo_id");
            entity.Property(e => e.Permissao)
                .HasDefaultValue(true)
                .HasColumnName("permissao");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");
        });
         
        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("utilizadores_pkey");

            entity.ToTable("utilizadores", "acesso");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.PalavraPasse)
                .HasMaxLength(100)
                .HasColumnName("palavra_passe");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("utilizador");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
