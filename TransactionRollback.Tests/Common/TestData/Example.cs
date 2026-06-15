namespace TransactionRollback.Tests.Common.TestData
{
    public static class Example
    {
        public static readonly List<Tables.Example> Rows =
        [
            new() { Column = 1 },
            new() { Column = 2 },
            new() { Column = 3 },
            new() { Column = 4 }
        ];
    }
}
