

namespace ArchivosMan.BLL.DTO
{
    public class ProyectoDto
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IconoId { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool EsActivo { get; set; } = true;
    }
}
