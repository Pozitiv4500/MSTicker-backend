using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    [Key]
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}
    