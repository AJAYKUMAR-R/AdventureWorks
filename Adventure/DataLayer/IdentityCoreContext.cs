using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Data;

public class IdentityCoreDBContext : IdentityDbContext
{
    public IdentityCoreDBContext()
    {
    }

    public IdentityCoreDBContext(DbContextOptions options) : base(options)
    {
    }
}