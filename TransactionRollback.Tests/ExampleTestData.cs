using TransactionRollback.Npoco;
using TransactionRollback.Tests.Common.TestData;

namespace TransactionRollback.Tests;

public class ExampleTestData : TransactionRollback.EntityFramework.ITestData, ITestData
{
    public string TableName => "Example";
    public IReadOnlyList<string> Columns => ["Column"];
    public IReadOnlyList<object[]> Rows =>
        Example.Rows.Select(r => new object[] { r.Column }).ToList();
}
