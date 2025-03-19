using Microsoft.AspNetCore.Identity;

namespace Shared.Identity.Entities;

public class SystemUser : IdentityUser
{
    public required string DisplayName { get; set; }
}