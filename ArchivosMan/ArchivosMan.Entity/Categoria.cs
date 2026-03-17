using System.ComponentModel.DataAnnotations.Schema;

namespace ArchivosMan.Entity
{
    [Table("Categoria")]
    public partial class Categoria
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }

        public virtual ICollection<Archivo> Archivos { get; set; } = new List<Archivo>();
    }
}
