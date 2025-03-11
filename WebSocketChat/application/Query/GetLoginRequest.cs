
using MediatR;
using Microsoft.EntityFrameworkCore;
public record GetLoginRequest(string Email, string Password) : IRequest<bool>;

public class GetLoginRequestHandler : IRequestHandler<GetLoginRequest, bool>
{
    private readonly WebSocketDb _db;
    public GetLoginRequestHandler(WebSocketDb db)
    {
        _db = db;
    }

    public async Task<bool> Handle(GetLoginRequest request, CancellationToken cancellationToken)
    {
        var users = await _db.Users
                  .Where(u => u.Email == request.Email && u.Password == request.Password)
                  .AnyAsync(cancellationToken); 

        return users;
    }
}
