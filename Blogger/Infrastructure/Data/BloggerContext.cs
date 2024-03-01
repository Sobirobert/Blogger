﻿
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BloggerContext : DbContext
{
    public BloggerContext(DbContextOptions<BloggerContext> options) : base(options)
    {

    }

    public DbSet<Post> Posts { get; set; }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
               .Entries()
               .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((AuditableEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((AuditableEntity)entityEntry.Entity).Created = DateTime.UtcNow;
            }
        }
        return await base.SaveChangesAsync();
    }

}
