using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.RefreshToken;
using Application.Authentication.Commands.Register;
using Carter;
using MediatR;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class AuthenticationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("authentications").WithTags("Authentications");

        group.MapPost("login", Login)
            .Produces(StatusCodes.Status200OK, typeof(AuthenticatedResponse))
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
        
        group.MapPost("register", Register)
            .Produces(StatusCodes.Status200OK, typeof(AuthenticatedResponse))
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
        
        group.MapPost("refresh", Refresh)
            .Produces(StatusCodes.Status200OK, typeof(AuthenticatedResponse))
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();;
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
    
    private static async Task<IResult> Refresh(
        HttpContext context,
        ISender sender,
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }
}
