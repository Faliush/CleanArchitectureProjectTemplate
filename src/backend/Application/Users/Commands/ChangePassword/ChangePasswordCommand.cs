using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid Id, string CurrentPassword, string NewPassword) : ICommand;
