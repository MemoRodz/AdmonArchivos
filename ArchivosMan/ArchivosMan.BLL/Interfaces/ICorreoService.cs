using System;
using System.Collections.Generic;
using System.Text;

namespace ArchivosMan.BLL.Interfaces
{
    public interface ICorreoService
    {
        Task<bool> EnviarCorreo(string CorreoDestino, string Asunto, string Mensaje);
    }
}
