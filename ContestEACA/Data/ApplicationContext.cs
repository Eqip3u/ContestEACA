﻿using Microsoft.EntityFrameworkCore;
using ContestEACA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ContestEACA.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Nomination> Nominations { get; set; }

        public DbSet<NewsContest> News { get; set; }

        public DbSet<ModerateUserContest> ModerateUsersContests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {     
            base.OnModelCreating(builder);
        }
    }
}
