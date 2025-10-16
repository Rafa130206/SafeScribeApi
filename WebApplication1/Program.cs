using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SafeScribe.Data;
using SafeScribe.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Configuração da autenticação JWT
builder.Services.AddAuthentication(options =>
{
    // Define o esquema de autenticação padrão como JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configuração dos parâmetros de validação do token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida se a chave de assinatura do token é válida
        // A chave secreta é convertida para bytes e usada para verificar a assinatura
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        
        // Valida se o emissor (Issuer) do token é o correto
        // Garante que o token foi emitido pela nossa aplicação
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        
        // Valida se a audiência (Audience) do token é a correta
        // Garante que o token é destinado à nossa aplicação
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        
        // Valida se o token não expirou
        // Verifica se o token ainda está dentro do prazo de validade
        ValidateLifetime = true,
        
        // Define o relógio de validação para UTC
        // Garante que a validação de tempo seja consistente independente do fuso horário
        ClockSkew = TimeSpan.Zero
    };
});

// Configuração da autorização
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de autenticação deve vir antes da autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.OpenConnection();
        Console.WriteLine("Conex�o com Oracle bem-sucedida!");
        db.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao conectar no Oracle: " + ex.Message);
    }
}

app.Run();
