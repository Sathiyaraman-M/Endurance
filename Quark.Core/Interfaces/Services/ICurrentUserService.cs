namespace Quark.Core.Interfaces.Services;

public interface ICurrentUserService
{
    public string UserId { get; }
    public string UserName { get; set; }
}