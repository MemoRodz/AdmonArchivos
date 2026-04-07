using Microsoft.EntityFrameworkCore;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Implementacion
{
    public class ProyectoService : IProyectoService
    {
        private readonly IGenericRepository<Proyecto> _repoProyecto;

        public ProyectoService(IGenericRepository<Proyecto> repoProyecto)
        {
            _repoProyecto = repoProyecto;
        }
        public async Task<Proyecto?> ActualizarAsync(Proyecto proyecto, int usuarioActualiza)
        {
            var existente = await _repoProyecto.Obtener(p => p.IdProyecto == proyecto.IdProyecto);
            if (existente == null) return null;

            existente.Nombre = proyecto.Nombre;
            existente.FechaActualiza = DateTime.Now;
            existente.UsuarioActualiza = usuarioActualiza;

            await _repoProyecto.Editar(existente);
            return existente;
        }

        public async Task<Proyecto> CrearAsync(Proyecto proyecto, int usuarioCrea)
        {
            proyecto.EsActivo = true;
            proyecto.FechaCrea = DateTime.Now;
            proyecto.UsuarioCrea = usuarioCrea;

            return await _repoProyecto.Crear(proyecto);
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var proyectoEliminar = await _repoProyecto.Obtener(p => p.IdProyecto == id && p.EsActivo);
            if (proyectoEliminar == null) return false;

            proyectoEliminar.EsActivo = false;
            proyectoEliminar.FechaActualiza = DateTime.Now;
            proyectoEliminar.UsuarioActualiza = usuarioActualiza;

            await _repoProyecto.Editar(proyectoEliminar);
            return true;
        }

        public async Task<List<Proyecto>> ListarAsync()
        {
            return await _repoProyecto.Consultar(p => p.EsActivo)
                .OrderByDescending(p => p.FechaCrea)
                .ToListAsync();
        }

        public async Task<Proyecto?> ObtenerPorIdAsync(int id)
        {
            return await _repoProyecto.Obtener(p => p.IdProyecto == id && p.EsActivo);
        }
    }
}
