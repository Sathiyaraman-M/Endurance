using System.ComponentModel.DataAnnotations;

namespace Quark.Core.Requests.Identity;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}