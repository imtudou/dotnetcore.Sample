using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Entity.Models;



namespace User.API.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { 
           
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserPropery>().Property(u=>u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserPropery>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserPropery>()
                .ToTable("UserProperies")
                .HasKey(u => new { u.AppUserId,u.Value,u.Key});

            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(u =>new { u.UserId,u.Tag});

            modelBuilder.Entity<BPFile>()
                .ToTable("UserBPFiles")
                .HasKey(u => u.Id);
        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserPropery> UserProperies { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<BPFile> BPFiles { get; set; }


    }
}
