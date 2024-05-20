using Domain.Entities;
namespace Application.Abstractions.Authentication.Jwt;

public interface IJwtProvider
{
    Task<string> Generate(User user);
}
