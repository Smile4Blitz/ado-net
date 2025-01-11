public IEnumerable<Example> ExecuteStoredProcedure(string procedureName, object parameters)
{
    using var db = new SqlConnection(_connectionString);
    return db.Query<Example>(procedureName, parameters, commandType: CommandType.StoredProcedure);
}
