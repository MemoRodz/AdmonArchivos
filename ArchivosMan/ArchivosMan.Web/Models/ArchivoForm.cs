using System.ComponentModel.DataAnnotations;

namespace ArchivosMan.Web.Models
{
    public class ArchivoForm
    {
        public int IdArchivo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL es obligatoria.")]
        [Url(ErrorMessage = "La URL no es válida.")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int CategoriaId { get; set; }
    }
}
