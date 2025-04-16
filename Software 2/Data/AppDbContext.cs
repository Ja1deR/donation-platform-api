using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Software_2.Models;

namespace Software_2.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<CategoriaDonacion> CategoriaDonacions { get; set; }

    public virtual DbSet<ComentariosValoracione> ComentariosValoraciones { get; set; }

    public virtual DbSet<Configuracione> Configuraciones { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Donacione> Donaciones { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<FundacionDocumento> FundacionDocumentos { get; set; }

    public virtual DbSet<Fundación> Fundacións { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Publicacione> Publicaciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TipoActividad> TipoActividads { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("DbContext no configurado. Asegúrese de configurar el DbContext en el contenedor de inyección de dependencias.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__F6FFFB8CDB49D4A3");

            entity.Property(e => e.IdAuditoria).HasColumnName("ID_auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Detalles).HasColumnType("text");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_accion");
            entity.Property(e => e.IdRegistroAfectado).HasColumnName("ID_registro_afectado");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Tabla_afectada");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auditoria_Usuarios");
        });

        modelBuilder.Entity<CategoriaDonacion>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaDonacion).HasName("PK__Categori__A114E90C7B3A1776");

            entity.ToTable("Categoria_Donacion");

            entity.Property(e => e.IdCategoriaDonacion).HasColumnName("ID_categoria_donacion");
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripción)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_categoria");
        });

        modelBuilder.Entity<ComentariosValoracione>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__4DF668A631E18AE2");

            entity.ToTable("Comentarios_Valoraciones");

            entity.Property(e => e.IdComentario).HasColumnName("ID_comentario");
            entity.Property(e => e.Aprobado).HasDefaultValue(false);
            entity.Property(e => e.Comentario).HasColumnType("text");
            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_comentario");
            entity.Property(e => e.IdDonacion).HasColumnName("ID_donacion");
            entity.Property(e => e.IdFundacion).HasColumnName("ID_fundacion");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.Reportado).HasDefaultValue(false);
            entity.Property(e => e.Respuesta).HasColumnType("text");

            entity.HasOne(d => d.IdDonacionNavigation).WithMany(p => p.ComentariosValoraciones)
                .HasForeignKey(d => d.IdDonacion)
                .HasConstraintName("FK_Comentarios_Valoraciones_Donaciones");

            entity.HasOne(d => d.IdFundacionNavigation).WithMany(p => p.ComentariosValoraciones)
                .HasForeignKey(d => d.IdFundacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentarios_Valoraciones_Fundación");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ComentariosValoraciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentarios_Valoraciones_Usuarios");
        });

        modelBuilder.Entity<Configuracione>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracion).HasName("PK__Configur__90679C80EF052937");

            entity.Property(e => e.IdConfiguracion).HasColumnName("ID_configuracion");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripción)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__B6ED5B3ED2F4B3DB");

            entity.ToTable("Documento");

            entity.Property(e => e.IdDocumento).HasColumnName("ID_documento");
            entity.Property(e => e.RutaDoc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Ruta_doc");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Tipo_documento");
        });

        modelBuilder.Entity<Donacione>(entity =>
        {
            entity.HasKey(e => e.IdDonacion).HasName("PK__Donacion__D588707BC603EBC8");

            entity.Property(e => e.IdDonacion).HasColumnName("ID_donacion");
            entity.Property(e => e.DescripciónDonacion)
                .HasColumnType("text")
                .HasColumnName("Descripción_donacion");
            entity.Property(e => e.FechaDonacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_donacion");
            entity.Property(e => e.IdCategoriaDonacion).HasColumnName("ID_categoria_donacion");
            entity.Property(e => e.IdEstado).HasColumnName("ID_estado");
            entity.Property(e => e.IdFundacion).HasColumnName("ID_fundacion");
            entity.Property(e => e.IdPublicacion).HasColumnName("ID_publicacion");
            entity.Property(e => e.IdUsuarioDonante).HasColumnName("ID_usuario_donante");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaDonacionNavigation).WithMany(p => p.Donaciones)
                .HasForeignKey(d => d.IdCategoriaDonacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donaciones_Categoria_Donacion");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Donaciones)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donaciones_Estado");

            entity.HasOne(d => d.IdFundacionNavigation).WithMany(p => p.Donaciones)
                .HasForeignKey(d => d.IdFundacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donaciones_Fundación");

            entity.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.Donaciones)
                .HasForeignKey(d => d.IdPublicacion)
                .HasConstraintName("FK_Donaciones_Publicaciones");

            entity.HasOne(d => d.IdUsuarioDonanteNavigation).WithMany(p => p.Donaciones)
                .HasForeignKey(d => d.IdUsuarioDonante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donaciones_Usuarios");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__BDA876FE0E42CB61");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("ID_estado");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_estado");
        });

        modelBuilder.Entity<FundacionDocumento>(entity =>
        {
            entity.HasKey(e => e.IdFundacionDocumento).HasName("PK__Fundacio__231018117EC74A1D");

            entity.ToTable("Fundacion_Documento");

            entity.Property(e => e.IdFundacionDocumento).HasColumnName("ID_fundacion_documento");
            entity.Property(e => e.IdDocumento).HasColumnName("ID_documento");
            entity.Property(e => e.IdFundacion).HasColumnName("ID_fundacion");

            entity.HasOne(d => d.IdDocumentoNavigation).WithMany(p => p.FundacionDocumentos)
                .HasForeignKey(d => d.IdDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fundacion_Documento_Documento");

            entity.HasOne(d => d.IdFundacionNavigation).WithMany(p => p.FundacionDocumentos)
                .HasForeignKey(d => d.IdFundacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fundacion_Documento_Fundación");
        });

        modelBuilder.Entity<Fundación>(entity =>
        {
            entity.HasKey(e => e.IdFundacion).HasName("PK__Fundació__DC9FE4B8AEB06F64");

            entity.ToTable("Fundación");

            entity.Property(e => e.IdFundacion).HasColumnName("ID_fundacion");
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripción).HasColumnType("text");
            entity.Property(e => e.Dirección)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_registro");
            entity.Property(e => e.IdTipoActividad).HasColumnName("ID_tipo_actividad");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.Nif)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NIF");
            entity.Property(e => e.NombreLegal)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Nombre_legal");
            entity.Property(e => e.SitioWeb)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Sitio_web");

            entity.HasOne(d => d.IdTipoActividadNavigation).WithMany(p => p.Fundacións)
                .HasForeignKey(d => d.IdTipoActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fundación_Tipo_Actividad");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Fundacións)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fundación_Usuarios");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__99BC7E5E05435B87");

            entity.Property(e => e.IdNotificacion).HasColumnName("ID_notificacion");
            entity.Property(e => e.Enviada).HasDefaultValue(false);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_envio");
            entity.Property(e => e.IdDonacion).HasColumnName("ID_donacion");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.Mensaje).HasColumnType("text");
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_notificacion");

            entity.HasOne(d => d.IdDonacionNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdDonacion)
                .HasConstraintName("FK_Notificaciones_Donaciones");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");
        });

        modelBuilder.Entity<Publicacione>(entity =>
        {
            entity.HasKey(e => e.IdPublicacion).HasName("PK__Publicac__1E838F30552D3200");

            entity.Property(e => e.IdPublicacion).HasColumnName("ID_publicacion");
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripción).HasColumnType("text");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_inicio");
            entity.Property(e => e.IdCategoriaDonacion).HasColumnName("ID_categoria_donacion");
            entity.Property(e => e.IdFundacion).HasColumnName("ID_fundacion");
            entity.Property(e => e.MetaCantidad).HasColumnName("Meta_cantidad");
            entity.Property(e => e.NombrePublicacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Nombre_publicacion");

            entity.HasOne(d => d.IdCategoriaDonacionNavigation).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.IdCategoriaDonacion)
                .HasConstraintName("FK_Publicaciones_Categoria_Donacion");

            entity.HasOne(d => d.IdFundacionNavigation).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.IdFundacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicaciones_Fundación");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__182A541257759658");

            entity.Property(e => e.IdRol).HasColumnName("ID_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_rol");
        });

        modelBuilder.Entity<TipoActividad>(entity =>
        {
            entity.HasKey(e => e.IdTipoActividad).HasName("PK__Tipo_Act__A0909E146C0D0245");

            entity.ToTable("Tipo_Actividad");

            entity.Property(e => e.IdTipoActividad).HasColumnName("ID_tipo_actividad");
            entity.Property(e => e.NombreTipoActividad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_tipo_actividad");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__Tipo_Doc__FEB84E2351A2976D");

            entity.ToTable("Tipo_Documento");

            entity.Property(e => e.IdTipoDocumento).HasColumnName("ID_tipo_documento");
            entity.Property(e => e.NombreTipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_tipo_documento");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__DF3D4252DE2473BB");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Apellido_usuario");
            entity.Property(e => e.ContraseñaUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Contraseña_usuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Correo_usuario");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_registro");
            entity.Property(e => e.IdRol).HasColumnName("ID_rol");
            entity.Property(e => e.IdTipoDocumento).HasColumnName("ID_tipo_documento");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_usuario");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Numero_documento");
            entity.Property(e => e.TelUsuario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tel_usuario");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("datetime")
                .HasColumnName("Ultimo_acceso");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Tipo_Documento");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
