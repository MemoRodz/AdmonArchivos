namespace ArchivosMan.Entity
{
    public partial class Rol
    {
        public int IdRol { get; set; }

        public string? Descripcion { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
