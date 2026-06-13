using Microsoft.EntityFrameworkCore;

namespace TransactionRollback.EntityFramework
{
    public static class Connection
    {
        public static ExampleDbContext Get(string connectionString) =>
            new(new DbContextOptionsBuilder<ExampleDbContext>().UseSqlServer(connectionString).Options);

        public static async Task RunInRolledBackTransactionAsync(this ExampleDbContext context, Func<ExampleDbContext, Task> test)
        {
            await context.Database.BeginTransactionAsync();
            try
            {
                await test(context);
            }
            finally
            {
                await context.Database.RollbackTransactionAsync();
            }
        }

        public static async Task Setup(ExampleDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Example");
            foreach (var row in Common.TestData.Example.Rows)
                await context.Database.ExecuteSqlRawAsync("INSERT INTO Example ([Column]) VALUES ({0})", row.Column);
        }
    }
}
