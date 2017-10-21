using Microsoft.EntityFrameworkCore;
using ContestEACA.Models;

namespace ContestEACA.Data
{
    public class ApplicationPostDbContext : DbContext
    {

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Nomination> Nominations { get; set; }

        public ApplicationPostDbContext(DbContextOptions<ApplicationPostDbContext> options)
            : base(options)
        {

        }
    }
}
