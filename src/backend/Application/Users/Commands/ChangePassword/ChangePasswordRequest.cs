namespace Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
