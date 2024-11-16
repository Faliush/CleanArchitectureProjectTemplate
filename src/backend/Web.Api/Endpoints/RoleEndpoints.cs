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

        group.MapPost("", Create)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequirePermission(Permissions.FullAccess);
        
        group.MapDelete("{id:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequirePermission(Permissions.FullAccess);
        
        group.MapGet("", GetAll)
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(Permissions.FullAccess);
        
        group.MapGet("permissions", GetAllPermissions)
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(Permissions.FullAccess);
    }
    
    private static async Task<IResult> Create(
        HttpContext context,
        ISender sender,
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsOk ? Results.Ok() : result.ToBadRequestProblem();
    }
    
    private static async Task<IResult> Delete(
        HttpContext context,
        ISender sender,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoleCommand(id), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }
    
    private static async Task<IResult> GetAll(
        HttpContext context,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllRolesQuery(), cancellationToken);

        return Results.Ok(result.Value);
    }
    
    private static async Task<IResult> GetAllPermissions(
        HttpContext context,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllPermissionsQuery(), cancellationToken);

        return Results.Ok(result.Value);
    }
}
