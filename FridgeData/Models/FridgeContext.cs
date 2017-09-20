﻿using System;
using FridgeData.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FridgeData.Models
{
    public class FridgeContext : IdentityDbContext<AppUser, IdentityRole, string>, IFridgeContext
    {
        public FridgeContext(DbContextOptions<FridgeContext> options) : base(options)
        { }

        public FridgeContext()
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public new DbSet<User> Users { get; set; }

        public DbSet<SeasonTotal> SeasonTotals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>().ToTable("games").HasKey(g => g.Id);
            builder.Entity<Pick>().ToTable("picks").HasKey(p => p.Id);
            builder.Entity<User>().ToTable("users").HasKey(u => u.Id);
            builder.Entity<SeasonTotal>().ToTable("v_season_totals").HasKey(st => st.UserId);
            builder.Entity<IdentityUserClaim<string>>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<IdentityRoleClaim<string>>().Property(x => x.Id).ValueGeneratedOnAdd();
            // shadow properties
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var password = Environment.GetEnvironmentVariable("FRIDGE_DB_PASSWORD");
            builder.UseMySql($"server=localhost;port=3306;database=fridgefootball;user=fridgefootball;password={password};Pooling=True");
        }

    }
}
