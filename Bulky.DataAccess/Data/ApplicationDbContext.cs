﻿using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category {ID =1,Name="Action",DisplayOrder=1 },
                new Category { ID = 2, Name = "Scifi", DisplayOrder = 2 },
                new Category { ID = 3, Name = "Romance", DisplayOrder = 3 }
                );
        }

    }
}
