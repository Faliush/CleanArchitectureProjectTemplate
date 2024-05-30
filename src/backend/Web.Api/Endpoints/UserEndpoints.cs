using Application.Abstractions.Authentication.Attribute;
using Application.Users.Commands.AddRoles;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.RemoveRoles;
using Application.Users.Commands.Update;
using Application.Users.Queries.GetUserById;
using Carter;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");

        group.MapGet("{id:guid}", GetById);
        group.MapPut("{id:guid}/change-password", ChangePassword);
        group.MapPut("{id:guid}", Update);
        group.MapPost("{id:guid}/roles", AddRoles);
        group.MapDelete("{id:guid}/roles", RemoveRoles);
    }

    [HasPermission(Permissions.User)]
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

    [HasPermission(Permissions.User)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> ChangePassword(
        HttpContext context,
        ISender sender,
        [FromRoute]Guid Id,
        [FromBody]ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ChangePasswordCommand(Id, request.CurrentPassword, request.NewPassword), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.User)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Update(
        HttpContext context,
        ISender sender,
        [FromRoute] Guid Id,
        [FromBody]UpdateUserRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateUserCommand(Id, request.FirstName, request.LastName), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.ManageUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> AddRoles(
        HttpContext httpContext,
        ISender sender,
        [FromRoute]Guid Id,
        [FromBody]AddRolesToUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new AddRolesToUserCommand(Id, request.RoleIds), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.ManageUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> RemoveRoles(
        HttpContext httpContext,
        ISender sender,
        [FromRoute] Guid Id, 
        [FromBody] RemoveRolesFromUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RemoveRolesFromUserCommand(Id, request.RoleIds), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }
}
