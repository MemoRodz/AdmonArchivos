
namespace ArchivosMan.BLL.DTO
{
    public class ContactoDto
    {
        public int IdContacto { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Telefono { get; set; }
        public bool EsActivo { get; set; } = true;
    }
}
