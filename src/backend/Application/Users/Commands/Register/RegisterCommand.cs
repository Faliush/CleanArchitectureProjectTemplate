using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int RoleId) : ICommand<Result<string>>;
