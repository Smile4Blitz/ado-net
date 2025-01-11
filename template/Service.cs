/* ExampleService.cs */
using System.Collections.Generic;

public interface IExampleService
{
    Example GetExampleById(int id);
    IEnumerable<Example> GetAllExamples();
    Example CreateExample(Example example);
    bool UpdateExample(int id, Example example);
    bool DeleteExample(int id);
}

public class ExampleService : IExampleService
{
    private readonly IExampleRepository _repository;

    public ExampleService(IExampleRepository repository)
    {
        _repository = repository;
    }

    public Example GetExampleById(int id) => _repository.GetById(id);

    public IEnumerable<Example> GetAllExamples() => _repository.GetAll();

    public Example CreateExample(Example example) => _repository.Create(example);

    public bool UpdateExample(int id, Example example) => _repository.Update(id, example);

    public bool DeleteExample(int id) => _repository.Delete(id);
}