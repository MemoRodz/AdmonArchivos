using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArchivosMan.BLL.Implementacion;
using ArchivosMan.BLL.Interfaces;
using ArchivosMan.BLL.Services;
using ArchivosMan.DAL.DbContext;
using ArchivosMan.DAL.Implementacion;
using ArchivosMan.DAL.Interfaces;

namespace ArchivosMan.IOC
{
    public static class Dependencia
    {
        public static IServiceCollection AddSistema(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ContextDb");

            services.AddDbContextFactory<ArchivosContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IFirebaseConfigService, FirebaseConfigService>();
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

            return services;
        }
    }
}
