public async Task<IEnumerable<Example>> GetExamplesAsync() {
    using var connection = new SqlConnection(_connectionString);
    var sql = "SELECT * FROM Examples";
    return await connection.QueryAsync<Example>(sql);
}
