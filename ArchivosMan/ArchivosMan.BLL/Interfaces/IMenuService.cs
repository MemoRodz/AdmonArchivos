using ArchivosMan.BLL.DTO;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuItemDto>> ObtenerMenuPorRolAsync(int rolId);
    }
}
