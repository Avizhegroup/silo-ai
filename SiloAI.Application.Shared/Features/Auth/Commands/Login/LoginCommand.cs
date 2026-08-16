namespace SiloAI.Application.Shared.Features;

public class LoginCommand : IRequest<LoginVm>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
