using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIGApi.Models;

public partial class ProjectoContext : DbContext
{
    public ProjectoContext()
    {
    }

    public ProjectoContext(DbContextOptions<ProjectoContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<ArvoreCartografia> ArvoreCartografia { get; set; }

    public virtual DbSet<Cartografialayer> Cartografialayers { get; set; }

    public virtual DbSet<Cartografia> Cartografia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=projecto;Username=postgres;Password=toor;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
         
        modelBuilder.Entity<Cartografialayer>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("cartografialayers_pkey");

            entity.ToTable("cartografialayers", "inf_geografica");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Layer)
                .HasMaxLength(256)
                .HasColumnName("layer");
            entity.Property(e => e.Parent).HasColumnName("parent");
        });
     

        modelBuilder.Entity<Cartografia>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("cartografia_pkey");

            entity.HasMany(c => c.Layers)        // Cartografia tem muitas Layers
                  .WithOne()   // Cada Layer tem uma Cartografia (pai)
                  .HasForeignKey(l => l.Parent) // A chave estrangeira em Layer é ParentRecId
                  .OnDelete(DeleteBehavior.Cascade); // Opcional: define o comportamento de exclusão em cascata

            entity.ToTable("cartografia", "inf_geografica");

            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(256)
                .HasColumnName("nome");
            entity.Property(e => e.Ordem).HasColumnName("ordem");
            entity.Property(e => e.Parent).HasColumnName("parent");
        });

       // modelBuilder.Entity<ArvoreCartografia>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
