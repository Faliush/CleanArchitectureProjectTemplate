using System.Text.RegularExpressions;

namespace Application.Abstractions.Validation;

internal static class ValidationRules
{
    internal static class User
    {
        internal const int FirstNameMaxLength = 100;
        internal const int LastNameMaxLength = 100;
        internal const int EmailMaxLength = 100;
        internal const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
    } 
}
