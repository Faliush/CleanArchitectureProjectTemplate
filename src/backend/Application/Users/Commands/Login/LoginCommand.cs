using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<Result<string>>;
