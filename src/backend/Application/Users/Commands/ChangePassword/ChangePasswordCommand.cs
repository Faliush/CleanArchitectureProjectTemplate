using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid Id, string Password) : ICommand<Result>;
