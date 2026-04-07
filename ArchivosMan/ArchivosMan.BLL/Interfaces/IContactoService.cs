using ArchivosMan.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArchivosMan.BLL.Interfaces
{
    public interface IContactoService
    {
        Task<Contacto?> ActualizarAsync(Contacto contacto, int usuarioActualiza);
        Task<Contacto> CrearAsync(Contacto contacto, int usuarioCrea);
        Task<bool> EliminarAsync(int id, int usuarioActualiza);
        Task<List<Contacto>> ListarAsync();
        Task<Contacto?> ObtenerPorIdAsync(int id);
    }
}
