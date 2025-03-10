public interface IuserUseCase
{
    Task<Result<Guid>> Execute(UserDto Value);
}