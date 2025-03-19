using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Identity.Entities;

namespace IdentityLibrary.Identity.Context;

public class ChessContext : IdentityDbContext<SystemUser>
{
    public ChessContext(DbContextOptions options) : base(options) {}
}