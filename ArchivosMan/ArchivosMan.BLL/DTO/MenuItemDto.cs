
namespace ArchivosMan.BLL.DTO
{
    public class MenuItemDto
    {
        public int IdMenu { get; set; }
        public string Descripcion { get; set; } = "";
        public string? Icono { get; set; }
        public string? Controlador { get; set; }
        public string? PaginaAccion { get; set; }
        public int? MenuPadreId { get; set; }
        public string Url =>
            string.IsNullOrWhiteSpace(Controlador)
            ? (PaginaAccion ?? "#")
            : $"/{Controlador}/{PaginaAccion}".ToLowerInvariant();

        public List<MenuItemDto> Hijos { get; set; } = new();
    }
}
