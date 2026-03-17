namespace ArchivosMan.Entity
{
    public partial class Proyecto
    {
        public int IdProyecto { get; set; }

        public string Nombre { get; set; } = null!;

        public int? IconoId { get; set; }

        public string? Descripcion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual Archivo? Icono { get; set; }
    }
}
