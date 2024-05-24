namespace Domain.Enums;

[Flags]
public enum Permissions
{
    None = 0,

    ManageUsers = 1 << 0, //1

    User = 1 << 1, //2

    // << 2 // 4

    All = ~None
}
