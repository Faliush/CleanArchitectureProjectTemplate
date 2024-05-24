using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password)
        : ICommand<Result<AuthenticatedResponse>>;
