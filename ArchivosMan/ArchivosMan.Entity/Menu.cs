namespace ArchivosMan.Entity
{
    public partial class Menu
    {
        public int IdMenu { get; set; }

        public string? Descripcion { get; set; }

        public int? MenuPadreId { get; set; }

        public string? Icono { get; set; }

        public string? Controlador { get; set; }

        public string? PaginaAccion { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual ICollection<Menu> InverseMenuPadre { get; set; } = new List<Menu>();

        public virtual Menu? MenuPadre { get; set; }

        public virtual ICollection<RolMenu> RolMenus { get; set; } = new List<RolMenu>();
    }
}
