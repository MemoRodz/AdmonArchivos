namespace ArchivosMan.Entity
{
    public partial class Negocio
    {
        public int IdNegocio { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string? UrlLogo { get; set; }

        public string? NombreLogo { get; set; }

        public string? Direccion { get; set; }

        public string Codpos { get; set; } = null!;

        public string Pais { get; set; } = null!;

        public string? Telefono { get; set; }

        public decimal? PorcentajeImpuesto { get; set; }

        public string? SimboloMoneda { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public string? NumeroDocumento { get; set; }

        public virtual ICollection<Configuracion> Configuracion { get; set; } = new List<Configuracion>();
    }
}
