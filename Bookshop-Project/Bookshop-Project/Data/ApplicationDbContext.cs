using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bookshop_Project.Models;
using Bookshop_Project.Data;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("AspNetUsers").HasKey(u => u.Id);
        modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles").HasKey(r => r.Id);
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles").HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins").HasKey(ul => new { ul.LoginProvider, ul.ProviderKey, ul.UserId });
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens").HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" }
        );
    }
}
