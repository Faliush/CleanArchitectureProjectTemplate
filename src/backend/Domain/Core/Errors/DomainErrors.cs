using Domain.Core.Primitives.Result;

namespace Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error InvalidCredentials => new("User.InvalidCredentials", "Invalid email or password");
        public static Error NotFound => new("User.NotFound", "User not found");
        public static Error InvalidRefreshToken => new("User.InvalidRefreshToken", "Invalid refresh token or expired time");
        public static Error InvalidClaims => new("User.InvalidClaims", "Invalid claims");
    }

    public static class Role 
    {
        public static Error NotFound => new("Role.NotFound", "Given role doesn't exist");
        public static Error NullOrEmpty => new("Role.NullOrEmpty", "Name is required");
        public static Error LongerThanAllowed => new("RoleLongerThanAllowed", "Role name is longer than allowed");
    }

    public static class FirstName
    {
        public static Error NullOrEmpty => new("FirstName.NullOrEmpty", "First name is required");
        public static Error LongerThanAllowed => new("FirstName.LongerThanAllowed", "First name is longer than allowed");
    }
    
    public static class LastName
    {
        public static Error NullOrEmpty => new("LastName.NullOrEmpty", "Last name is required");
        public static Error LongerThanAllowed => new("LastName.LongerThanAllowed", "Last name is longer than allowed");
    }

    public static class Email
    {
        public static Error NullOrEmpty => new("Email.NullOrEmpty", "Email is required");
        public static Error LongerThanAllowed => new("Email.LongerThanAllowed", "The email is longer than allowed");
        public static Error InvalidFormat => new("Email.InvalidFormat", "The email format is invalid");
    }

    public static class Password
    {
        public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

        public static Error TooShort => new("Password.TooShort", "The password is too short.");

        public static Error MissingUppercaseLetter => new(
            "Password.MissingUppercaseLetter",
            "The password requires at least one uppercase letter.");

        public static Error MissingLowercaseLetter => new(
            "Password.MissingLowercaseLetter",
            "The password requires at least one lowercase letter.");

        public static Error MissingDigit => new(
            "Password.MissingDigit",
            "The password requires at least one digit.");

        public static Error MissingNonAlphaNumeric => new(
            "Password.MissingNonAlphaNumeric",
            "The password requires at least one non-alphanumeric.");
    }

}
