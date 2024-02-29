using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BloggerContext : DbContext
{
    public BloggerContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
}