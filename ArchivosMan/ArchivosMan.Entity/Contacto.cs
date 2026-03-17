using System;
using System.Collections.Generic;
using System.Text;

namespace ArchivosMan.Entity
{
    public partial class Contacto
    {
        public int IdContacto { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string? Telefono { get; set; }

        public bool EsActivo { get; set; } = true;

        public DateTime? FechaCrea { get; set; }

        public int UsuarioCrea { get; set; }

        public DateTime? FechaActualiza { get; set; }

        public int? UsuarioActualiza { get; set; }
    }
}
