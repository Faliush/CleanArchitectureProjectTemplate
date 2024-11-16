using Application.Roles.Commands.Create;
using Application.Roles.Commands.Delete;
using Application.Roles.Queries.GetAll;
using Application.Roles.Queries.GetAllPermissions;
using Application.Users.Queries.GetUserById;
using Carter;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Authentication.Attribute;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class RoleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("roles").WithTags("Roles");

        group.MapPost("", Create);
        group.MapDelete("{id:int}", Delete);
        group.MapGet("", GetAll);
        group.MapGet("permissions", GetAllPermissions);
    }

    [HasPermission(Permissions.ManageRoles)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Create(
        HttpContext context,
        ISender sender,
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.ManageRoles)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> Delete(
        HttpContext context,
        ISender sender,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoleCommand(id), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.ReadRolesPermissions)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> GetAll(
        HttpContext context,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllRolesQuery(), cancellationToken);

        return result.IsOk ? Results.Ok() : result.ToBadRequestProblem();
    }

    [HasPermission(Permissions.ReadRolesPermissions)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> GetAllPermissions(
        HttpContext context,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllPermissionsQuery(), cancellationToken);

        return result.IsOk ? Results.Ok() : result.ToBadRequestProblem();
    }
}
