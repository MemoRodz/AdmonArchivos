namespace ArchivosMan.Entity
{
    public partial class Configuracion
    {
        public int IdConfiguracion { get; set; }

        public string? Recurso { get; set; }

        public string? Propiedad { get; set; }

        public string? Valor { get; set; }

        public int? NegocioId { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual Negocio? Negocio { get; set; }
    }
}
