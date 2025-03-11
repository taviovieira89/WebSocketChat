public interface ICreateUserUseCase
{
    Task<Result<Guid>> Execute(UserDto Value, CancellationToken cancellationToken);
}