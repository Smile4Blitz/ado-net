public void BulkInsert(DataTable table)
{
    using var connection = new SqlConnection(_connectionString);
    using var bulkCopy = new SqlBulkCopy(connection);
    bulkCopy.DestinationTableName = "YourTable";
    connection.Open();
    bulkCopy.WriteToServer(table);
    connection.Close();
}
