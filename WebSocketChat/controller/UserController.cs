
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IuserUseCase _createUseCase;

    public UserController(IuserUseCase createUseCase)
    {
        _createUseCase = createUseCase;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        try
        {
            var result = await _createUseCase.Execute(userDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return CreatedAtAction(nameof(CreateUser), new { id = result.Value }, result.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro inesperado ao criar user.");
        }

    }

}