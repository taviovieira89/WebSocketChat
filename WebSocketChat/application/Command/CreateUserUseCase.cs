public class CreateUserUseCase : ICreateUserUseCase
{
    private readonly WebSocketDb _db;

    public CreateUserUseCase(WebSocketDb db)
    {
        _db = db;
    }

    public async Task<Result<Guid>> Execute(UserDto Value, CancellationToken cancellationToken)
    {
        var userResult = User.Create(Value.Email, Value.Password);

        if (!userResult.IsSuccess)
        {
            return Result<Guid>.Failure(userResult.Error);
        }
        
        var user = userResult.Value;
        _db.Users.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.IdUser);
    }

}