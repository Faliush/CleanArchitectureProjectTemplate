using Application.Authentication.Commands;
using Application.Authentication.Commands.GoogleSignIn;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.RefreshToken;
using Application.Authentication.Commands.Register;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class AuthenticationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("authentications").WithTags("Authentications");

        group.MapPost("login", Login);
        group.MapPost("register", Register);
        group.MapPost("refresh", Refresh);
        group.MapPost("google-signin", GoogleSignIn);
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Login(
        HttpContext context,
        ISender sender,
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Register(
        HttpContext context,
        ISender sender,
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> GoogleSignIn(
        HttpContext context,
        ISender sender,
        GoogleSignInCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToBadRequestProblem();
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
