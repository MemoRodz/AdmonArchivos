namespace ArchivosMan.Entity
{
    public partial class Archivo
    {
        public int IdArchivo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Url { get; set; } = null!;

        public int CategoriaId { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}
