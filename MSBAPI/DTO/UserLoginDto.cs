using System.ComponentModel.DataAnnotations;

namespace MSBAPI.DTO;

public class UserLoginDto
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}