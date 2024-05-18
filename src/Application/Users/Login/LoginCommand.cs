using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Login;

public record LoginCommand(string Email, string Password) : ICommand<Result<string>>;
