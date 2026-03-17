namespace ArchivosMan.Entity
{
    public partial class RolMenu
    {
        public int IdRolMenu { get; set; }

        public int? IdRol { get; set; }

        public int? IdMenu { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual Menu? IdMenuNavigation { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
