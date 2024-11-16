using Domain.ValueObjects;

namespace Application.Abstractions.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
