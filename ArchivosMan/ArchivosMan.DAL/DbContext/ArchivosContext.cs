using Microsoft.EntityFrameworkCore;
using ArchivosMan.Entity;

namespace ArchivosMan.DAL.DbContext
{
    public partial class ArchivosContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ArchivosContext() {}

        public ArchivosContext(DbContextOptions<ArchivosContext> options)
            : base(options) { }

        #region Entidades
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<RolMenu> RolesMenus => Set<RolMenu>();
        public DbSet<Negocio> Negocios => Set<Negocio>();
        public DbSet<Configuracion> Configuraciones => Set<Configuracion>();
        public DbSet<Pais> Pais => Set<Pais>();
        public DbSet<Estado> Estados => Set<Estado>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Archivo> Archivos => Set<Archivo>();
        public DbSet<Proyecto> Proyectos => Set<Proyecto>();
        public DbSet<Contacto> Contactos => Set<Contacto>();
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archivo>(entity =>
            {
                entity.HasKey(e => e.IdArchivo).HasName("PK__Archivo_Id");

                entity.ToTable("Archivo");

                entity.Property(e => e.IdArchivo).HasColumnName("idArchivo");
                entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("url");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.Categoria).WithMany(p => p.Archivos)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK__Archivo__Categoria_Id");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");
                entity.HasKey(e => e.IdCategoria).HasName("PK__Categoria_Id");

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.IdConfiguracion).HasName("PK__Configuracion_Id");

                entity.ToTable("Configuracion");

                entity.Property(e => e.IdConfiguracion).HasColumnName("idConfiguracion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.NegocioId).HasColumnName("negocioId");
                entity.Property(e => e.Propiedad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propiedad");
                entity.Property(e => e.Recurso)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("recurso");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
                entity.Property(e => e.Valor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("valor");

                entity.HasOne(d => d.Negocio).WithMany(p => p.Configuracion)
                    .HasForeignKey(d => d.NegocioId)
                    .HasConstraintName("FK__Configuracion__Negocio_Id");
            });

            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.HasKey(e => e.IdContacto).HasName("PK__Contacto_Id");

                entity.ToTable("Contacto");

                entity.Property(e => e.IdContacto).HasColumnName("idContacto");
                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado).HasName("PK__Estado_Id");

                entity.ToTable("Estado");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.PaisId).HasColumnName("paisId");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.Pais).WithMany(p => p.Estados)
                    .HasForeignKey(d => d.PaisId)
                    .HasConstraintName("FK__Estado__pais_Id");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu).HasName("PK__Menu_Id");

                entity.ToTable("Menu");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");
                entity.Property(e => e.Controlador)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("controlador");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Icono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("icono");
                entity.Property(e => e.MenuPadreId).HasColumnName("menuPadreId");
                entity.Property(e => e.PaginaAccion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("paginaAccion");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.MenuPadre).WithMany(p => p.InverseMenuPadre)
                    .HasForeignKey(d => d.MenuPadreId)
                    .HasConstraintName("FK__Menu__menuPadre_Id");
            });

            modelBuilder.Entity<Negocio>(entity =>
            {
                entity.HasKey(e => e.IdNegocio).HasName("PK__Negocio_Id");

                entity.ToTable("Negocio");

                entity.Property(e => e.IdNegocio).HasColumnName("idNegocio");
                entity.Property(e => e.Codpos)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("codpos");
                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");
                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direccion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.NombreLogo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombreLogo");
                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("numeroDocumento");
                entity.Property(e => e.Pais)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pais");
                entity.Property(e => e.PorcentajeImpuesto)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("porcentajeImpuesto");
                entity.Property(e => e.SimboloMoneda)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("simboloMoneda");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
                entity.Property(e => e.UrlLogo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("urlLogo");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais).HasName("PK__Pais_Id");

                entity.Property(e => e.IdPais).HasColumnName("idPais");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasKey(e => e.IdProyecto).HasName("PK__Proyecto_Id");

                entity.ToTable("Proyecto");

                entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaFin");
                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaInicio");
                entity.Property(e => e.IconoId).HasColumnName("iconoId");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.Icono).WithMany(p => p.Proyectos)
                    .HasForeignKey(d => d.IconoId)
                    .HasConstraintName("FK__Proyecto__icono_Id");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol).HasName("PK__Rol_Id");

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol).HasColumnName("idRol");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");
            });

            modelBuilder.Entity<RolMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolMenu).HasName("PK__RolMenu_Id");

                entity.ToTable("RolMenu");

                entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.IdMenu).HasColumnName("idMenu");
                entity.Property(e => e.IdRol).HasColumnName("idRol");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("FK__RolMenu__idMenu_Id");

                entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__RolMenu__idRol_Id");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario_Id");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
                entity.Property(e => e.Clave)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clave");
                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");
                entity.Property(e => e.EsActivo).HasColumnName("esActivo");
                entity.Property(e => e.FechaActualiza)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaActualiza");
                entity.Property(e => e.FechaCrea)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCrea");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.RolId).HasColumnName("rolId");
                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("urlFoto");
                entity.Property(e => e.UsuarioActualiza).HasColumnName("usuarioActualiza");
                entity.Property(e => e.UsuarioCrea).HasColumnName("usuarioCrea");

                entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_Usuario__Rol_Id");
            });

            //Borrado lógico:
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.EsActivo);
            modelBuilder.Entity<Rol>().HasQueryFilter(r => r.EsActivo);
            modelBuilder.Entity<Menu>().HasQueryFilter(m => m.EsActivo);
            modelBuilder.Entity<RolMenu>().HasQueryFilter(rm => rm.EsActivo);
            modelBuilder.Entity<Negocio>().HasQueryFilter(n => n.EsActivo);
            modelBuilder.Entity<Configuracion>().HasQueryFilter(c => c.EsActivo);
            modelBuilder.Entity<Pais>().HasQueryFilter(p => p.EsActivo);
            modelBuilder.Entity<Estado>().HasQueryFilter(e => e.EsActivo);
            modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.EsActivo);
            modelBuilder.Entity<Archivo>().HasQueryFilter(a => a.EsActivo);
            modelBuilder.Entity<Proyecto>().HasQueryFilter(p => p.EsActivo);
            modelBuilder.Entity<Contacto>().HasQueryFilter(c => c.EsActivo);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
