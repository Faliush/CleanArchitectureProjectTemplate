using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.Update;

public sealed record UpdateUserCommand(Guid Id, string FirstName, string LastName) : ICommand<Result>;
