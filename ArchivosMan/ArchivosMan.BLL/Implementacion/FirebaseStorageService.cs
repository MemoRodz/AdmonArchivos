using ArchivosMan.BLL.Interfaces;
using ArchivosMan.DAL.Interfaces;
using ArchivosMan.Entity;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;

namespace ArchivosMan.BLL.Implementacion
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IFirebaseConfigService _configService;
        private readonly IGenericRepository<Categoria> _repoCategoria;

        public FirebaseStorageService(IFirebaseConfigService configService, IGenericRepository<Categoria> repoCategoria)
        {
            _configService = configService;
            _repoCategoria = repoCategoria;
        }
        public async Task<string> SubirStorage(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo)
        {
            string UrlImagen = string.Empty;
            try
            {
                var cfg = await _configService.ObtenerConfigAsync();

                // Configurar el cliente de autenticación (nuevo en 4.x)
                var firebaseAuthConfig = new FirebaseAuthConfig
                {
                    ApiKey = cfg.ApiKey,
                    AuthDomain = cfg.AuthDomain,     // PROJECT_ID.firebasestorage.app
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
                };
                var authClient = new FirebaseAuthClient(firebaseAuthConfig);
                // Iniciar sesión con email / clave
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(cfg.Email, cfg.Clave);
                // Obtener el IdToken (equivalente al antiguo FirebaseToken)
                var firebaseToken = await userCredential.User.GetIdTokenAsync();
                // Subir al Storage como antes de 4.x
                var cancellation = new CancellationTokenSource();
                var task = new FirebaseStorage(
                    cfg.Ruta,     // PROJECT_ID.appspot.com
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(CarpetaDestino)
                    .Child(NombreArchivo)
                    .PutAsync(StreamArchivo, cancellation.Token);
                UrlImagen = await task;
            }
            catch (Exception)
            {
                UrlImagen = string.Empty;
            }
            return UrlImagen;
        }
        public async Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo)
        {
            try
            {
                var cfg = await _configService.ObtenerConfigAsync();
                var firebaseAuthConfig = new FirebaseAuthConfig
                {
                    ApiKey = cfg.ApiKey,
                    AuthDomain = cfg.AuthDomain,
                    Providers = new FirebaseAuthProvider[]
    {
                        new EmailProvider()
    }
                };
                var authClient = new FirebaseAuthClient(firebaseAuthConfig);
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(cfg.Email, cfg.Clave);
                var firebaseToken = await userCredential.User.GetIdTokenAsync();

                var cancellation = new CancellationTokenSource();
                var task = new FirebaseStorage(
                    cfg.Ruta,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(CarpetaDestino)
                    .Child(NombreArchivo)
                    .DeleteAsync();
                await task;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> SubirArchivoAsync(Stream streamArchivo, int categoriaId, string nombreArchivo)
        {
            string urlImagen = string.Empty;
            try
            {
                //Obtener Categoría
                var categoria = await _repoCategoria.Obtener(c => c.IdCategoria == categoriaId && c.EsActivo);
                if (categoria == null)
                    throw new InvalidOperationException("La categoría seleccionada no existe o está inactiva.");
                // Obtener configuración general de Firebase
                var cfg = await _configService.ObtenerConfigAsync();
                // Resolver carpeta por categoría 
                var carpetaDestino = await _configService.ObtenerCarpetaPorCategoriaAsync(categoria.Nombre);
                // Autenticación
                var firebaseAuthConfig = new FirebaseAuthConfig
                {
                    ApiKey = cfg.ApiKey,
                    AuthDomain = cfg.AuthDomain,
                    Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
                };
                var authClient = new FirebaseAuthClient(firebaseAuthConfig);
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(cfg.Email, cfg.Clave);
                var firebaseToken = await userCredential.User.GetIdTokenAsync();
                // Subir a Firebase Storage
                var cancellation = new CancellationTokenSource();
                var task = new FirebaseStorage(
                    cfg.Ruta,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken),
                        ThrowOnCancel = true
                    })
                .Child(carpetaDestino)
                .Child(nombreArchivo)
                .PutAsync(streamArchivo, cancellation.Token);

                urlImagen = await task;
            }
            catch (Exception ex)
            {
                urlImagen = string.Empty;
            }
            return urlImagen;
        }

    }
}
