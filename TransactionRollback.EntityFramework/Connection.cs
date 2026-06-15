using Microsoft.EntityFrameworkCore;

namespace TransactionRollback.EntityFramework
{
    public static class Connection
    {
        public static TContext Get<TContext>(string connectionString) where TContext : DbContext =>
            (TContext)Activator.CreateInstance(typeof(TContext),
                new DbContextOptionsBuilder<TContext>().UseSqlServer(connectionString).Options)!;

        public static async Task RunInRolledBackTransactionAsync<TContext>(this TContext context, Func<TContext, Task> test) where TContext : DbContext
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

        public static async Task Setup(DbContext context, ITestData testData)
        {
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM {testData.TableName}");
            var cols = string.Join(", ", testData.Columns.Select(c => $"[{c}]"));
            var placeholders = string.Join(", ", Enumerable.Range(0, testData.Columns.Count).Select(i => $"{{{i}}}"));
            var insertSql = $"INSERT INTO {testData.TableName} ({cols}) VALUES ({placeholders})";
            foreach (var row in testData.Rows)
                await context.Database.ExecuteSqlRawAsync(insertSql, row);
        }
    }
}
