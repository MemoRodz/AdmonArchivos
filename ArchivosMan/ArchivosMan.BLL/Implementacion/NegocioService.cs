
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class NegocioService : INegocioService
    {
        private readonly IGenericRepository<Negocio> _repoNegocio;
        public NegocioService(IGenericRepository<Negocio> repoNegocio)
        {
            _repoNegocio = repoNegocio;
        }
        public async Task<Negocio?> ActualizarAsync(Negocio negocio, int usuarioActualiza)
        {
            var existente = await _repoNegocio.Obtener(n => n.IdNegocio == negocio.IdNegocio);
            if (existente == null) return null;

            existente.Nombre = negocio.Nombre;
            existente.Correo = negocio.Correo;
            existente.UrlLogo = negocio.UrlLogo;
            existente.NombreLogo = negocio.NombreLogo;
            existente.Direccion = negocio.Direccion;
            existente.Codpos = negocio.Codpos;
            existente.Pais = negocio.Pais;
            existente.Telefono = negocio.Telefono;
            existente.PorcentajeImpuesto = negocio.PorcentajeImpuesto;
            existente.NumeroDocumento = negocio.NumeroDocumento;

            await _repoNegocio.Editar(existente);
            return existente;
        }

        public async Task<Negocio> CrearAsync(Negocio negocio, int usuarioCrea)
        {
            negocio.EsActivo = true;
            negocio.FechaCrea = DateTime.Now;
            negocio.UsuarioCrea = usuarioCrea;

            return await _repoNegocio.Crear(negocio);
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var negocioEliminar = await _repoNegocio.Obtener(n => n.IdNegocio == id && n.EsActivo);
            if (negocioEliminar == null) return false;

            negocioEliminar.EsActivo = false;
            negocioEliminar.FechaActualiza = DateTime.Now;
            negocioEliminar.UsuarioActualiza = usuarioActualiza;

            await _repoNegocio.Editar(negocioEliminar);
            return true;
        }

        public async Task<List<Negocio>> ListarAsync()
        {
            return await _repoNegocio.Consultar(n => n.EsActivo)
                .OrderByDescending(n => n.FechaCrea)
                .ToListAsync();
        }

        public async Task<Negocio?> ObtenerPorIdAsync(int id)
        {
            return await _repoNegocio.Obtener(n => n.IdNegocio == id && n.EsActivo);
        }
    }
}
