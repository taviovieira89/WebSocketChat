using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatureWebSocketChat(this IServiceCollection services)
    {
        services.AddSingleton<WebSocketConnectionManager>();
        return services;
    }

    public static IServiceCollection AddWebCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin() // Permite qualquer origem (não recomendado para produção)
                      .AllowAnyMethod() // Permite qualquer método (GET, POST, etc.)
                      .AllowAnyHeader(); // Permite qualquer cabeçalho
            });
        });
        return services;
    }

    public static IServiceCollection AddAuthenticationWebSockets(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration.GetConnectionString("Key").Decode();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = "WebSocketChat",
                                    ValidAudience = "ChatFrontend",
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
                                };
                            });
        return services;
    }

    public static WebApplication AddWebSocketChat(this WebApplication app)
    {
        app.Map("/ws", async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var connectionManager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();

            // Extrai o roomId da query string
            var roomId = context.Request.Query["room"].ToString();
            if (string.IsNullOrEmpty(roomId))
            {
                logger.LogWarning("Solicitação WebSocket sem roomId.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                connectionManager.AddConnection(roomId, webSocket);
                logger.LogInformation("Nova conexão WebSocket na sala {RoomId}. Conexões ativas: {ConnectionsCount}", roomId, connectionManager.GetConnectionsCount(roomId));

                await HandleWebSocketConnection(webSocket, connectionManager, logger, roomId);
            }
            else
            {
                logger.LogWarning("Solicitação WebSocket inválida.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        });
        return app;
    }

    private static async Task HandleWebSocketConnection(WebSocket webSocket, WebSocketConnectionManager connectionManager, ILogger logger, string roomId)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                // Recebe a mensagem do cliente
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // Decodifica a mensagem recebida
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    logger.LogInformation("Mensagem recebida na sala {RoomId}: {Message}", roomId, message);

                    // Broadcast da mensagem para todos os clientes na mesma sala
                    await connectionManager.BroadcastAsync(roomId, message);
                    logger.LogInformation("Mensagem broadcastada para a sala {RoomId}.", roomId);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    // Fecha a conexão e remove o WebSocket da sala
                    logger.LogInformation("Conexão WebSocket fechada pelo cliente na sala {RoomId}.", roomId);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    connectionManager.RemoveConnection(roomId, webSocket);
                    logger.LogInformation("Conexão removida da sala {RoomId}. Conexões ativas: {ConnectionsCount}", roomId, connectionManager.GetConnectionsCount(roomId));
                }
            }
            catch (Exception ex)
            {
                // Log de erros e remoção da conexão em caso de falha
                logger.LogError(ex, "Erro ao processar a conexão WebSocket na sala {RoomId}.", roomId);
                connectionManager.RemoveConnection(roomId, webSocket);
                logger.LogInformation("Conexão removida da sala {RoomId} devido a um erro. Conexões ativas: {ConnectionsCount}", roomId, connectionManager.GetConnectionsCount(roomId));
                break;
            }
        }
    }
}