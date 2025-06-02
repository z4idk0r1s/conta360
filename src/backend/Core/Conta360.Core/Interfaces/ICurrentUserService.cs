namespace Conta360.Core.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        bool IsInRole(string roleName);
    }
}