
using MediatR;
using Microsoft.EntityFrameworkCore; // Adicione esta linha
public record GetUserRequest() : IRequest<IList<User>>;

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, IList<User>>
{
    private readonly WebSocketDb _db;
    public GetUserRequestHandler(WebSocketDb db)
    {
        _db = db;
    }

    public async Task<IList<User>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var users = await _db.Users.ToListAsync(cancellationToken);

        return users; 
    }
}
