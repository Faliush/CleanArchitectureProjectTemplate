using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<Result<AuthenticatedResponse>>;
