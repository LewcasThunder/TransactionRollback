using NUnit.Framework;
using TransactionRollback.Npoco;
using TransactionRollback.Tests.Common;

namespace TransactionRollback.Tests
{
    [TestFixture]
    public class NPoco
    {
        [Test]
        public async Task TransactionShouldInsertTestDataAndRollBackCorrectly()
        {
            await using var databaseConnection = Connection.Get(Configuration.ConnectionString);
            var countBefore = await databaseConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Example");

            await databaseConnection.RunInRolledBackTransactionAsync(async database =>
            {
                Connection.Setup(database, new ExampleTestData());
                Assert.That(
                    await database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Example"),
                    Is.EqualTo(Common.TestData.Example.Rows.Count));
            });

            var countAfter = await databaseConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Example");
            Assert.That(countAfter, Is.EqualTo(countBefore));
        }
    }
}
