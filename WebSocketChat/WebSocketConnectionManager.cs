using System.Net.WebSockets;
using System.Text;

public class WebSocketConnectionManager
{
    private readonly Dictionary<string, List<WebSocket>> _rooms = new();

    public void AddConnection(string roomId, WebSocket webSocket)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            _rooms[roomId] = new List<WebSocket>();
        }
        _rooms[roomId].Add(webSocket);
    }

    public void RemoveConnection(string roomId, WebSocket webSocket)
    {
        if (_rooms.ContainsKey(roomId))
        {
            _rooms[roomId].Remove(webSocket);
            if (_rooms[roomId].Count == 0)
            {
                _rooms.Remove(roomId);
            }
        }
    }

    public async Task BroadcastAsync(string roomId, string message)
    {
        if (_rooms.ContainsKey(roomId))
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            foreach (var connection in _rooms[roomId])
            {
                if (connection.State == WebSocketState.Open)
                {
                    await connection.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

    public int GetConnectionsCount(string roomId)
    {
        return _rooms.ContainsKey(roomId) ? _rooms[roomId].Count : 0;
    }
}