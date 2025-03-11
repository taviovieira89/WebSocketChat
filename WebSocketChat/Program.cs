using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
// Configuração de serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebScoket API", Version = "v1" });
});
// Configurar o sistema de logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Adiciona logs no console
var key = builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddAuthenticationWebSockets(key);
builder.Services.AddAuthorization();
builder.Services.AddFeatureWebSocketChat(builder.Configuration);
builder.Services.AddWebCors();
// Registrar controllers
builder.Services.AddControllers();

var app = builder.Build();

// Habilita o middleware de CORS
app.UseCors("AllowAllOrigins");

app.UseWebSockets();

app.AddWebSocketChat();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();