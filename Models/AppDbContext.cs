using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace EcommerceAPI.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
    }
}
