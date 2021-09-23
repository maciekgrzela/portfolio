using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<Repo> Repos { get; set; }
        public DbSet<TechnologyStackInfo> TechnologyStackInfos { get; set; }
        public DbSet<TechnologyStackItem> TechnologyStackItems { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Repo>()
                .HasMany(p => p.Links)
                .WithOne(p => p.Repo)
                .HasForeignKey(p => p.RepoId);

            builder.Entity<Repo>()
                .HasMany(p => p.TechnologyStackInfos)
                .WithOne(p => p.Repo)
                .HasForeignKey(p => p.RepoId);

            builder.Entity<TechnologyStackItemAndInfo>()
                .HasKey(ts => new {ts.TechnologyStackInfoId, ts.TechnologyStackItemId});

            builder.Entity<TechnologyStackItemAndInfo>()
                .HasOne(p => p.TechnologyStackInfo)
                .WithMany(p => p.TechnologyStackItemAndInfos)
                .HasForeignKey(p => p.TechnologyStackInfoId);

            builder.Entity<TechnologyStackItemAndInfo>()
                .HasOne(p => p.TechnologyStackItem)
                .WithMany(p => p.TechnologyStackItemAndInfos)
                .HasForeignKey(p => p.TechnologyStackItemId);

            builder.Entity<SocialMediaLink>()
                .HasOne(p => p.User)
                .WithMany(p => p.SocialMediaLinks)
                .HasForeignKey(p => p.UserId);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var currentDate = DateTime.Now;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = currentDate;
                    entry.Entity.LastModified = currentDate;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = currentDate;
                }
            }
        }
    }
}