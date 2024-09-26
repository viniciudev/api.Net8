using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContextClass>(options =>
            {
                //options.UseNpgsql("host=localhost;user id=postgres;password=123456789;database=Comercial3irmaos;Pooling=false;Timeout=300;CommandTimeout=300;");
                //options.UseNpgsql("host=89.117.146.50;user id=postgres;password=7A24Jdp1Rcyv;database=ComercialHomolog;Pooling=false;Timeout=300;CommandTimeout=300;");
                //options.UseNpgsql("host=localhost;user id=postgres;password=admin;database=4Axon;Pooling=false;Timeout=300;CommandTimeout=300;");
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("Infrastructure"));

            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClientRepository, ClientRepository>();
            return services;
        }
    }
}
