using Microsoft.EntityFrameworkCore;
 
namespace CSharp.Models
{
    public class CSharpContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public CSharpContext(DbContextOptions<CSharpContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Idea> ideas { get; set; }
        public DbSet<Like> likes { get; set; }
    }
}