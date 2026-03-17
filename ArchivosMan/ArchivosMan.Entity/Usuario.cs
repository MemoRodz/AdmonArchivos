namespace ArchivosMan.Entity
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public int? RolId { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public string? UrlFoto { get; set; }

        public virtual Rol? Rol { get; set; }
    }
}
