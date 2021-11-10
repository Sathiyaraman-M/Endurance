namespace Quark.Core.Responses.Identity;

public class PermissionResponse
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public List<RoleClaimResponse> RoleClaims { get; set; }
}