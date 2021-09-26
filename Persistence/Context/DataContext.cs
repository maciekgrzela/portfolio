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
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<ContactFormRequest> ContactFormRequests { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectTag>()
                .HasKey(pt => new {pt.ProjectId, pt.TagId});
            
            builder.Entity<ProjectTag>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTags)
                .HasForeignKey(pt => pt.ProjectId);  
            
            builder.Entity<ProjectTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(p => p.ProjectTags)
                .HasForeignKey(pt => pt.TagId);
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