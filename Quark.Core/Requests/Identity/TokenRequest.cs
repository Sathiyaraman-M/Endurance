using System.ComponentModel.DataAnnotations;

namespace Quark.Core.Requests.Identity;

public class TokenRequest
{
    [Required(ErrorMessage = "Please enter your username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }
}