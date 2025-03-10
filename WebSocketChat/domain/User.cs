public class User
{
    public Guid IdUser { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    private User() { }

    public static Result<User> Create(string email, string password)
    {

       if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Result<User>.Failure("O Campo Email e Senha não podem ser nulos ou vazios.");
        }

        var user =  new User
        {
            IdUser = Guid.NewGuid(),
            Email = email,
            Password = password
        };

        return Result<User>.Success(user);
    }
}