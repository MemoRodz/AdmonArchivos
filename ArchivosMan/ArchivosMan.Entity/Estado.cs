namespace ArchivosMan.Entity
{
    public partial class Estado
    {
        public int IdEstado { get; set; }

        public string Nombre { get; set; } = null!;

        public int? PaisId { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual Pais? Pais { get; set; }
    }
}
