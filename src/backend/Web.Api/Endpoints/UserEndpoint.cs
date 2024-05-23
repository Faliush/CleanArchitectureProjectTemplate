using Application.Users.Login;
using Application.Users.Register;
using Carter;
using MediatR;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class UserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");

        group.MapPost("login", Login);
        group.MapPost("register", Register);
    }

    private static async Task<IResult> Login(
        HttpContext context,
        ISender sender,
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }

    private static async Task<IResult> Register(
        HttpContext context,
        ISender sender,
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }
}
