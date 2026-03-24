using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArchivosMan.BLL.Implementacion
{
    public class FirebaseConfigService : IFirebaseConfigService
    {
        private readonly IGenericRepository<Configuracion> _repoConfig;

        public FirebaseConfigService(IGenericRepository<Configuracion> repoConfig)
        {
            _repoConfig = repoConfig;
        }
        public async Task<string> ObtenerCarpetaPorCategoriaAsync(string nombreCategoria)
        {
            var lista = await _repoConfig
                .Consultar(c => c.Recurso == "FireBase_Storage" && c.EsActivo).ToListAsync();

            var clave = "carpeta_" + nombreCategoria.ToUpperInvariant(); // ej. LOGO, USUARIO
            var valor = lista.FirstOrDefault(c =>
                c.Propiedad.Equals(clave, StringComparison.OrdinalIgnoreCase))?.Valor;

            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException($"No se encontró configuración '{clave}' para FireBase_Storage.");

            return valor;
        }

        public async Task<FirebaseStorageConfig> ObtenerConfigAsync()
        {
            var lista = await _repoConfig
                .Consultar(c => c.Recurso == Constantes.Servicios.Storage && c.EsActivo)
                .ToListAsync();

            string Get(string prop) =>
                lista.FirstOrDefault(c => c.Propiedad == prop)?.Valor
                ?? throw new InvalidOperationException($"Configuración '{prop}' para FireBase_Storage no está definida.");

            return new FirebaseStorageConfig
            {
                Email = Get("email"),
                Clave = Get("clave"),
                Ruta = Get("ruta"),
                ApiKey = Get("api_key"),
                AuthDomain = Get("auth_domain")
            };
        }
    }
}
