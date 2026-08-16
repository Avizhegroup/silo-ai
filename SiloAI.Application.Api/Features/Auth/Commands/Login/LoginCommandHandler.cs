using SiloAI.Identity.Server;
using LoginCommand = SiloAI.Application.Shared.Features.LoginCommand;

namespace SiloAI.Application.Api.Features;

public class LoginCommandHandler(IAiJwtService jwtService) : IRequestHandler<LoginCommand, LoginVm>
{
    public async Task<LoginVm> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await jwtService.AuthenticateAsync(request.Username, request.Password);

        if (token is null)
            return new LoginVm { Successful = false, Message = "Invalid credentials." };

        return new LoginVm { Successful = true, Token = token };
    }
}
