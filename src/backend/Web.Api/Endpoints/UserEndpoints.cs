using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.SetRoles;
using Application.Users.Commands.Update;
using Application.Users.Queries.GetUserById;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Extentions;

namespace Web.Api.Endpoints;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");

        group.MapGet("{id:guid}", GetById)
            .Produces(StatusCodes.Status200OK, typeof(UserResponse))
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        
        group.MapPut("{id:guid}/change-password", ChangePassword)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
        
        group.MapPut("{id:guid}", Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        
        group.MapPost("{id:guid}/roles", SetRoles)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
    
    private static async Task<IResult> GetById(
        HttpContext context,
        ISender sender,
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserByIdQuery(id), cancellationToken);

        return result.IsOk ? Results.Ok(result.Value) : result.ToNotFoundProblem();
    }
    
    private static async Task<IResult> ChangePassword(
        HttpContext context,
        ISender sender,
        [FromRoute] Guid id,
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ChangePasswordCommand(id, request.CurrentPassword, request.NewPassword), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }
    
    private static async Task<IResult> Update(
        HttpContext context,
        ISender sender,
        [FromRoute] Guid id,
        [FromBody] UpdateUserRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateUserCommand(id, request.FirstName, request.LastName), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToNotFoundProblem();
    }
    
    private static async Task<IResult> SetRoles(
        HttpContext httpContext,
        ISender sender,
        [FromRoute] Guid id,
        [FromBody] SetRolesToUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new SetRolesToUserCommand(id, request.RoleIds), cancellationToken);

        return result.IsOk ? Results.NoContent() : result.ToBadRequestProblem();
    }
}
