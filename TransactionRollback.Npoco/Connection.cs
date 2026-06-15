using Microsoft.Data.SqlClient;
using NPoco;

namespace TransactionRollback.Npoco
{
    public static class Connection
    {
        public static IDatabase Get(string connectionString) =>
            new Database(connectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance);

        public static async Task RunInRolledBackTransactionAsync(this IDatabase database, Func<IDatabase, Task> test)
        {
            using var transaction = database.GetTransaction();
            await test(database);
        }

        public static void Setup(IDatabase database, ITestData testData)
        {
            database.Execute($"DELETE FROM {testData.TableName}");
            var cols = string.Join(", ", testData.Columns.Select(c => $"[{c}]"));
            var placeholders = string.Join(", ", Enumerable.Range(0, testData.Columns.Count).Select(i => $"@{i}"));
            var insertSql = $"INSERT INTO {testData.TableName} ({cols}) VALUES ({placeholders})";
            foreach (var row in testData.Rows)
                database.Execute(insertSql, row);
        }
    }
}
