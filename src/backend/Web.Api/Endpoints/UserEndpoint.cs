using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetUserById;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class UserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");

        group.MapPost("login", Login);
        group.MapPost("register", Register);
        group.MapGet("{id:guid}", GetById);
        group.MapPut("{id:guid}/change-password", ChangePassword);
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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

    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    private static async Task<IResult> GetById(
        HttpContext context,
        ISender sender,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserByIdQuery(id), cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToNotFoundProblem();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> ChangePassword(
        HttpContext context,
        ISender sender,
        Guid Id,
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }
}
