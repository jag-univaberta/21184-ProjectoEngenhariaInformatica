using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;


namespace CadastroApi.Models;

public partial class ProjectoContext : DbContext
{
    public ProjectoContext()
    {
    }

    public ProjectoContext(DbContextOptions<ProjectoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppEstado> AppEstado { get; set; } 

    public virtual DbSet<Cemiterio> Cemiterios { get; set; }

    public virtual DbSet<Concessionario> Concessionarios { get; set; }

    public virtual DbSet<Construcao> Construcao { get; set; }

    public virtual DbSet<ConstrucaoConcessionario> ConstrucaoConcessionario { get; set; }
     
    public virtual DbSet<Movimento> Movimentos { get; set; }
     
    public virtual DbSet<Residente> Residente { get; set; }

    public virtual DbSet<Talhao> Talhao { get; set; }

    public virtual DbSet<TipoConstrucao> TipoConstrucao { get; set; }

    public virtual DbSet<TipoMovimento> TipoMovimento { get; set; }


    public virtual DbSet<Distrito> Distrito { get; set; }
    public virtual DbSet<Concelho> Concelho { get; set; }
    public virtual DbSet<Freguesia> Freguesia { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=projecto;Username=postgres;Password=toor;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<AppEstado>(entity =>
        {
            entity.HasKey(e => e.Utilizador_id ).HasName("pk_app_estado");

            entity.ToTable("app_estado", "cadastro");

            entity.Property(e => e.Utilizador_id).HasColumnName("utilizador_id");
            entity.Property(e => e.Definicoes) 
                .HasColumnName("definicoes"); 
        });

        modelBuilder.Entity<Cemiterio>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("cemiterios_pkey");

            entity.ToTable("cemiterios", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Dicofre)
                .HasMaxLength(100)
                .HasColumnName("dicofre");
            entity.Property(e => e.Morada)
                .HasMaxLength(100)
                .HasColumnName("morada");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Concessionario>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("concessionarios_pkey");

            entity.ToTable("concessionarios", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Contacto)
                .HasMaxLength(100)
                .HasColumnName("contacto");
            entity.Property(e => e.Dicofre)
                .HasMaxLength(100)
                .HasColumnName("dicofre");
            entity.Property(e => e.Morada)
                .HasMaxLength(100)
                .HasColumnName("morada");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Nif)
                  .HasMaxLength(9)
                  .HasColumnName("nif");
        });

        modelBuilder.Entity<Construcao>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("construcoes_pkey");

            entity.ToTable("construcoes", "cadastro");

            entity.HasIndex(e => e.Geometria, "construcoes_gis").HasMethod("gist");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
            entity.Property(e => e.Geometria).HasColumnName("geometria");
            entity.Property(e => e.TalhaoId).HasColumnName("talhao_id");
            entity.Property(e => e.TipoconstrucaoId).HasColumnName("tipoconstrucao_id");
        });

        modelBuilder.Entity<ConstrucaoConcessionario>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("construcoes_concessionarios_pkey");

            entity.ToTable("construcoes_concessionarios", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.ConcessionarioId).HasColumnName("concessionario_id");
            entity.Property(e => e.ConstrucaoId).HasColumnName("construcao_id");
            entity.Property(e => e.DataFim)
                .HasMaxLength(8)
                .HasColumnName("data_fim");
            entity.Property(e => e.DataInicio)
                .HasMaxLength(8)
                .HasColumnName("data_inicio");
        });

     

        modelBuilder.Entity<Movimento>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("movimentos_pkey");

            entity.ToTable("movimentos", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.ConstrucaodestinoId).HasColumnName("construcaodestino_id");
            entity.Property(e => e.DataMovimento)
                .HasMaxLength(8)
                .HasColumnName("data_movimento");
            entity.Property(e => e.ResidenteId).HasColumnName("residente_id");
            entity.Property(e => e.TipomovimentoId).HasColumnName("tipomovimento_id");
        });
         

        modelBuilder.Entity<Residente>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("residentes_pkey");

            entity.ToTable("residentes", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.DataFalecimento)
                .HasMaxLength(8)
                .HasColumnName("data_falecimento");
            entity.Property(e => e.DataInumacao)
                .HasMaxLength(8)
                .HasColumnName("data_inumacao");
            entity.Property(e => e.DataNascimento)
                .HasMaxLength(8)
                .HasColumnName("data_nascimento");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Talhao>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("talhao_pkey");

            entity.ToTable("talhao", "cadastro");

            entity.HasIndex(e => e.Geometria, "talhao_gis").HasMethod("gist");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.CemiterioId).HasColumnName("cemiterio_id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .HasColumnName("codigo");
            entity.Property(e => e.Geometria).HasColumnName("geometria");
        });

        modelBuilder.Entity<TipoConstrucao>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("tipos_construcao_pkey");

            entity.ToTable("tipos_construcao", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
            entity.Property(e => e.Observacao)
               .HasMaxLength(100)
               .HasColumnName("observacao");
            entity.Property(e => e.Movimentosn) 
                .HasColumnName("movimentos_sn");

        });

        modelBuilder.Entity<TipoMovimento>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("tipos_movimentos_pkey");

            entity.ToTable("tipos_movimentos", "cadastro");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
        });


        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("distritos_pkey");

            entity.ToTable("distritos", "public");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(128)
                .HasColumnName("nome");
            entity.Property(e => e.Di)
                .HasMaxLength(7)
                .HasColumnName("di");
        });

        modelBuilder.Entity<Concelho>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("concelhos_pkey");

            entity.ToTable("concelhos", "public");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(128)
                .HasColumnName("nome");
            entity.Property(e => e.Di)
                .HasMaxLength(7)
                .HasColumnName("di");
            entity.Property(e => e.Co)
                .HasMaxLength(7)
                .HasColumnName("co");
        });
        modelBuilder.Entity<Freguesia>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("distritos_pkey");

            entity.ToTable("freguesias", "public");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(128)
                .HasColumnName("nome");
            entity.Property(e => e.Di)
                .HasMaxLength(7)
                .HasColumnName("di");
            entity.Property(e => e.Co)
                .HasMaxLength(7)
                .HasColumnName("co");
            entity.Property(e => e.Fre)
                .HasMaxLength(7)
                .HasColumnName("fre");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
