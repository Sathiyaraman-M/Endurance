namespace Quark.Core.Requests.Identity;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public string UserId { get; set; }
}