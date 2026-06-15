using Microsoft.EntityFrameworkCore;
using TransactionRollback.Tests.Common.Tables;

namespace TransactionRollback.Tests;

public class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
{
    public DbSet<Example> Example => Set<Example>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>(entity =>
        {
            entity.ToTable("Example");
            entity.HasNoKey();
            entity.Property(example => example.Column).HasColumnName("Column");
        });
    }
}
