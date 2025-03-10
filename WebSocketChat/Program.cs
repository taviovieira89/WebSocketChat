using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configurar o sistema de logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Adiciona logs no console
var key = builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddAuthenticationWebSockets(key);
builder.Services.AddAuthorization();
builder.Services.AddFeatureWebSocketChat(builder.Configuration);
builder.Services.AddWebCors();

var app = builder.Build();

// Habilita o middleware de CORS
app.UseCors("AllowAllOrigins");

app.UseWebSockets();

app.AddWebSocketChat();

app.MapPost("/login", (LoginRequest request) =>
{
    Console.WriteLine($"User: {request.Email}, Password: {request.Password}");
    // Verifique as credenciais do usuário (exemplo simples)
    if (request.Email == "user" && request.Password == "password")
    {
        var token = new JwtSecurityToken(
            issuer: "ChatBackend",
            audience: "ChatFrontend",
            claims: new[] { new Claim(ClaimTypes.Name, request.Email) },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    return Results.Unauthorized();
});


app.Run();