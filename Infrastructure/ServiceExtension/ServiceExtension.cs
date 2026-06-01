using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure"));

			});

			// Configuração de JWT
			var jwtKey = configuration["Jwt:Key"];
			if (string.IsNullOrEmpty(jwtKey))
				throw new InvalidOperationException("JWT Key não foi configurada");

			var key = Encoding.ASCII.GetBytes(jwtKey);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidateAudience = true,
					ValidAudience = configuration["Jwt:Audience"],
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IClientRepository, ClientRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			return services;
		}
	}
}
