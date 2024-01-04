using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAplicacionTurnos.Models;

namespace WebAplicacionTurnos.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<EstadosTurno> EstadosTurnos { get; set; }

    public virtual DbSet<Localidad> Localidades { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TiposServicio> TiposServicios { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD087FCB8DC9D");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Barrio)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CuitCuil)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Partido)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Localidad).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.LocalidadId)
                .HasConstraintName("FK__Clientes__Locali__49C3F6B7");

            entity.HasOne(d => d.Provincia).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__Clientes__Provin__4AB81AF0");
        });

        modelBuilder.Entity<EstadosTurno>(entity =>
        {
            entity.HasKey(e => e.EstadoTurnoId).HasName("PK__EstadosT__10C65F061F7CED34");

            entity.ToTable("EstadosTurno");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadosT__92C53B6CD547AE10").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Localidad>(entity =>
        {
            entity.HasKey(e => e.LocalidadId).HasName("PK__Localida__6E2890A2E803340F");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Provincia).WithMany(p => p.Localidades)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__Localidad__Provi__3A81B327");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.ProvinciaId).HasName("PK__Provinci__F7CBC7770DDEF015");

            entity.HasIndex(e => e.Descripcion, "UQ__Provinci__92C53B6C6EDD3476").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEECC220FF2927");

            entity.HasIndex(e => e.Descripcion, "UQ__Servicio__92C53B6C8F5AC552").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Observacion).HasColumnType("text");
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.TipoServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicioId)
                .HasConstraintName("FK__Servicios__TipoS__403A8C7D");
        });

        modelBuilder.Entity<TiposServicio>(entity =>
        {
            entity.HasKey(e => e.TipoServicioId).HasName("PK__TiposSer__BC9FF47D86CED978");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.TurnoId).HasName("PK__Turnos__AD3E2E94F9F560A3");

            entity.Property(e => e.FechaTurno).HasColumnType("date");
            entity.Property(e => e.Observacion).HasColumnType("text");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Turnos__ClienteI__4E88ABD4");

            entity.HasOne(d => d.EstadoTurno).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.EstadoTurnoId)
                .HasConstraintName("FK__Turnos__EstadoTu__4D94879B");

            entity.HasOne(d => d.Servicio).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.ServicioId)
                .HasConstraintName("FK__Turnos__Servicio__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
