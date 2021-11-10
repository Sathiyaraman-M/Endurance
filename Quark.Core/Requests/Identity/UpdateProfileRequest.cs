using System.ComponentModel.DataAnnotations;

namespace Quark.Core.Requests.Identity;

public class UpdateProfileRequest
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string UserName { get; set; }

    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}