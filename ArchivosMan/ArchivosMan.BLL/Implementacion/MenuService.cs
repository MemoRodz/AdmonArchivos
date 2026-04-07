using ArchivosMan.BLL.DTO;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<ArchivosContext> _dbFactory;

        public MenuService(IDbContextFactory<ArchivosContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<List<MenuItemDto>> ObtenerMenuPorRolAsync(int rolId)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var rolMenus = await db.RolesMenus
                .Where(rm => rm.EsActivo && rm.IdRol == rolId)
                .ToListAsync();

            var idsMenu = rolMenus.Select(rm => rm.IdMenu).Distinct().ToList();

            var menus = await db.Menus
                .Where(m => m.EsActivo && idsMenu.Contains(m.IdMenu))
                .OrderBy(m => m.MenuPadreId)
                .ThenBy(m => m.Descripcion)
                .ToListAsync();

            var dict = menus.ToDictionary(
            m => m.IdMenu,
            m => new MenuItemDto
            {
                IdMenu = m.IdMenu,
                Descripcion = m.Descripcion ?? "",
                Icono = m.Icono,
                Controlador = m.Controlador,
                PaginaAccion = m.PaginaAccion,
                MenuPadreId = m.MenuPadreId
            });

            List<MenuItemDto> raiz = new List<MenuItemDto>();

            foreach (var item in dict.Values)
            {
                if (item.MenuPadreId.HasValue &&
                    dict.TryGetValue(item.MenuPadreId.Value, out var padre))
                {
                    padre.Hijos.Add(item);
                }
                else
                {
                    raiz.Add(item);
                }
            }

            return raiz;
        }
    }
}
