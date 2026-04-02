using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArchivosMan.BLL.Implementacion
{
    public class EstadoService : IEstadoService
    {
        private readonly ArchivosContext _db;

        public EstadoService(ArchivosContext db)
        {
            _db = db;
        }

        public async Task<Estado?> ActualizarAsync(Estado estado, int usuarioActualiza)
        {
            var existente = await _db.Estados.FirstOrDefaultAsync(e => e.IdEstado == estado.IdEstado);
            if (existente == null) return null;

            existente.Nombre = estado.Nombre;
            existente.PaisId = estado.PaisId;
            existente.FechaActualiza = DateTime.Now;
            existente.UsuarioActualiza = usuarioActualiza;

            await _db.SaveChangesAsync();
            return existente;
        }

        public async Task<Estado> CrearAsync(Estado estado, int usuarioCrea)
        {
            estado.EsActivo = true;
            estado.FechaCrea = DateTime.Now;
            estado.UsuarioCrea = usuarioCrea;

            _db.Estados.Add(estado);
            await _db.SaveChangesAsync();
            return estado;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioActualiza)
        {
            var estado = await _db.Estados.FirstOrDefaultAsync(e => e.IdEstado == id);
            if (estado == null) return false;

            estado.EsActivo = false;
            estado.FechaActualiza = DateTime.Now;
            estado.UsuarioActualiza = usuarioActualiza;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Estado>> ListarAsync()
        {
            return await _db.Estados
                .Include(e => e.Pais)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Estado>> ListarPorPaisAsync(int paisId)
        {
            return await _db.Estados
                .Where(e => e.PaisId == paisId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Estado?> ObtenerPorIdAsync(int id)
        {
            return await _db.Estados
                .Include(e => e.Pais)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdEstado == id);
        }
    }
}
