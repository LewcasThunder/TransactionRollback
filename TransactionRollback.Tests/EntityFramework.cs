using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TransactionRollback.EntityFramework;
using TransactionRollback.Tests.Common;

namespace TransactionRollback.Tests
{
    [TestFixture]
    public class EntityFramework
    {
        [Test]
        public async Task TransactionShouldInsertTestDataAndRollBackCorrectly()
        {
            await using var context = Connection.Get<ExampleDbContext>(Configuration.ConnectionString);
            var countBefore = await context.Example.CountAsync();

            await context.RunInRolledBackTransactionAsync(async databaseContext =>
            {
                await Connection.Setup(databaseContext, new ExampleTestData());
                Assert.That(
                    await databaseContext.Example.CountAsync(),
                    Is.EqualTo(Common.TestData.Example.Rows.Count));
            });

            var countAfter = await context.Example.CountAsync();
            Assert.That(countAfter, Is.EqualTo(countBefore));
        }
    }
}
