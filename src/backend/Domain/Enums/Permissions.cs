namespace Domain.Enums;

[Flags]
public enum Permissions
{
    None = 0,

    User = 1 << 0, //1

    ManageUsers = 1 << 1, //2

    ReadRolesPermissions = 1 << 2, // 4

    ManageRoles = 1 << 3, // 8

    // << 4 // 16

    All = ~None
}
