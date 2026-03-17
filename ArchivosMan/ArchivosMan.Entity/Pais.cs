namespace ArchivosMan.Entity
{
    public partial class Pais
    {
        public int IdPais { get; set; }

        public string Nombre { get; set; } = null!;

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual ICollection<Estado> Estados { get; set; } = new List<Estado>();
    }
}
