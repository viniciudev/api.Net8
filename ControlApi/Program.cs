using Infrastructure.ServiceExtension;
using Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDIServices(builder.Configuration);
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

// --- Configuração OpenAPI simplificada ---
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();

	// Configura o Swagger UI com suporte a JWT via JavaScript
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/openapi/v1.json", "Minha API v1");

		// Configura o Swagger UI para enviar o token JWT
		options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
	});
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

// Importante: Autenticação deve vir antes de Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();