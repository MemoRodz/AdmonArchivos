using Microsoft.EntityFrameworkCore;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;

namespace ArchivosMan.BLL.Implementacion
{
    public class ContactoService : IContactoService
    {
        private readonly IGenericRepository<Contacto> _repoContacto;

        public ContactoService(IGenericRepository<Contacto> repoContacto)
        {
            _repoContacto = repoContacto;
        }
        public async Task<Contacto?> ActualizarAsync(Contacto contacto, int usuarioActualiza)
        {
            var existente = await _repoContacto.Obtener(c => c.IdContacto == contacto.IdContacto);
            if (existente == null) return null;

            existente.Nombre = contacto.Nombre;
            existente.Correo = contacto.Correo;
            existente.Telefono = contacto.Telefono;
            existente.UsuarioActualiza = usuarioActualiza;

            await _repoContacto.Editar(existente);
            return existente;
        }

        public async Task<Contacto> CrearAsync(Contacto contacto, int usuarioCrea)
        {
            contacto.EsActivo = true;
            contacto.FechaCrea = DateTime.Now;
            contacto.UsuarioCrea = usuarioCrea;

            return await _repoContacto.Crear(contacto);
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var contactoEliminar = await _repoContacto.Obtener(c => c.IdContacto == id && c.EsActivo);
            if (contactoEliminar == null) return false;

            contactoEliminar.EsActivo = false;
            contactoEliminar.FechaActualiza = DateTime.Now;
            contactoEliminar.UsuarioActualiza = usuarioActualiza;

            await _repoContacto.Editar(contactoEliminar);
            return true;
        }

        public async Task<List<Contacto>> ListarAsync()
        {
            return await _repoContacto.Consultar(c => c.EsActivo)
                .OrderByDescending(c => c.FechaCrea)
                .ToListAsync();
        }

        public async Task<Contacto?> ObtenerPorIdAsync(int id)
        {
            return await _repoContacto.Obtener(c => c.IdContacto == id && c.EsActivo);
        }
    }
}
