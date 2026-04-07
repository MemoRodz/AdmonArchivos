

using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class PaisService : IPaisService
    {
        private readonly ArchivosContext _db;

        public PaisService(ArchivosContext db)
        {
            _db = db;
        }
        public async Task<Pais?> ActualizarAsync(Pais pais, int usuarioActualiza)
        {
            var existente = await _db.Pais.FirstOrDefaultAsync(p => p.IdPais == pais.IdPais);
            if (existente == null) return null;

            existente.Nombre = pais.Nombre;
            existente.FechaActualiza = DateTime.Now;
            existente.UsuarioActualiza = usuarioActualiza;

            await _db.SaveChangesAsync();
            return existente;
        }

        public async Task<Pais> CrearAsync(Pais pais, int usuarioCrea)
        {
            pais.EsActivo = true;
            pais.FechaCrea = DateTime.Now;
            pais.UsuarioCrea = usuarioCrea;

            _db.Pais.Add(pais);
            await _db.SaveChangesAsync();
            return pais;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var pais = await _db.Pais.FirstOrDefaultAsync(p => p.IdPais == id);
            if (pais == null) return false;

            pais.EsActivo = false;
            pais.FechaActualiza = DateTime.Now;
            pais.UsuarioActualiza = usuarioActualiza;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Pais>> ListarAsync()
        {
            return await _db.Pais
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Pais?> ObtenerPorIdAsync(int id)
        {
            return await _db.Pais
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPais == id);
        }
    }
}
