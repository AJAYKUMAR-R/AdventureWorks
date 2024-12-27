using Adventure.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Data;

//Newly added property should be inherited here.
public class IdentityCoreDBContext : IdentityDbContext<ApplicationUser>
{
    public IdentityCoreDBContext()
    {
    }

    public IdentityCoreDBContext(DbContextOptions options) : base(options)
    {
    }
}