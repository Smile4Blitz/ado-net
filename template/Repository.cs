/* ExampleRepository.cs */
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

public interface IExampleRepository
{
    Example GetById(int id);
    IEnumerable<Example> GetAll();
    Example Create(Example example);
    bool Update(int id, Example example);
    bool Delete(int id);
}

public class ExampleRepository : IExampleRepository
{
    private readonly string _connectionString;

    public ExampleRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public Example GetById(int id)
    {
        using var db = Connection;
        return db.QuerySingleOrDefault<Example>("SELECT * FROM Examples WHERE Id = @Id", new { Id = id });
    }

    public IEnumerable<Example> GetAll()
    {
        using var db = Connection;
        return db.Query<Example>("SELECT * FROM Examples");
    }

    public Example Create(Example example)
    {
        using var db = Connection;
        var id = db.ExecuteScalar<int>(
            "INSERT INTO Examples (Name, Description) VALUES (@Name, @Description); SELECT SCOPE_IDENTITY();",
            example);
        example.Id = id;
        return example;
    }

    public bool Update(int id, Example example)
    {
        using var db = Connection;
        var rowsAffected = db.Execute(
            "UPDATE Examples SET Name = @Name, Description = @Description WHERE Id = @Id",
            new { example.Name, example.Description, Id = id });
        return rowsAffected > 0;
    }

    public bool Delete(int id)
    {
        using var db = Connection;
        var rowsAffected = db.Execute("DELETE FROM Examples WHERE Id = @Id", new { Id = id });
        return rowsAffected > 0;
    }
}