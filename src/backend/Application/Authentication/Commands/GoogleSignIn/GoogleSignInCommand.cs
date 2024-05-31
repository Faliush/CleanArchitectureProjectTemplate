using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Authentication.Commands.GoogleSignIn;

public sealed record GoogleSignInCommand(string Provider, string IdToken) : ICommand<Result<AuthenticatedResponse>>;
