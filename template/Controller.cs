/* ExampleController.cs */
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/v1/examples")]
public class ExampleController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExampleController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpGet("{id}")]
    public ActionResult<Example> GetExampleById(int id)
    {
        var example = _exampleService.GetExampleById(id);
        if (example == null)
            return NotFound();

        return Ok(example);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Example>> GetAllExamples()
    {
        return Ok(_exampleService.GetAllExamples());
    }

    [HttpPost]
    public ActionResult<Example> CreateExample([FromBody] Example example)
    {
        var createdExample = _exampleService.CreateExample(example);
        return CreatedAtAction(nameof(GetExampleById), new { id = createdExample.Id }, createdExample);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExample(int id, [FromBody] Example example)
    {
        if (!_exampleService.UpdateExample(id, example))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExample(int id)
    {
        if (!_exampleService.DeleteExample(id))
            return NotFound();

        return NoContent();
    }
}