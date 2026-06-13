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

        public static void Setup(IDatabase database)
        {
            database.Execute("DELETE FROM Example");
            foreach (var row in Common.TestData.Example.Rows)
                database.Execute("INSERT INTO Example ([Column]) VALUES (@0)", row.Column);
        }
    }
}
