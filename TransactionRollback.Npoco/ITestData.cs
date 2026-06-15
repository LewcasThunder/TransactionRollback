namespace TransactionRollback.Npoco;

public interface ITestData
{
    string TableName { get; }
    IReadOnlyList<string> Columns { get; }
    IReadOnlyList<object[]> Rows { get; }
}
