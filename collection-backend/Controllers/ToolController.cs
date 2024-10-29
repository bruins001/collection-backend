using collection_backend.Models;
using collection_backend.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace collection_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : ControllerBase
    {

        private readonly IGenericRespository<Tool> _repository;
        public ToolController(IGenericRespository<Tool> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tool>>> GetToolPage(int page = 1, int limit = 20, string orderBy = "")
        {
            if (page <= 0)
            {
                throw new BadHttpRequestException($"Page number must be greater then 0 current page is {page}.");
            }

            if (limit <= 0)
            {
                throw new BadHttpRequestException($"Limit must be greater then 0 current limit is {limit}.");
            }

            var tools = await _repository.GetToolPageAsync(page, limit, orderBy);
            return Ok(tools);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Tool>>> GetToolById(int id)
        {
            Tool? tool = await _repository.GetOneByIdAsync(id);
            if (tool == null)
            {
                return BadRequest();
            }
            return Ok(tool);
        }

        [HttpGet("bulk")]
        public async Task<ActionResult<IEnumerable<Tool>>> GetToolsById([FromQuery] IEnumerable<int> ids)
        {
            if (ids.Count() > 1000)
            { // Don't want to make requests too large
                return BadRequest($"Get request too large. Maximum size of get is 1000 current size is { ids.Count() }.");
            }

            IEnumerable<Tool>? tools = await _repository.GetBulkByIdAsync(ids);
            if (tools == null)
            {
                return NotFound();
            }
            return Ok(tools);
        }

        [HttpPost]
        public async Task<ActionResult<Tool>> InsertTool(Tool tool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            await _repository.InsertOneAsync(tool);
            return CreatedAtAction(nameof(GetToolsById), new { ids = tool.Id }, tool);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<Tool>>> InsertToolBulk(IEnumerable<Tool> tools)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        if (tools.Count() > 1000)
            { // Don't want to make requests too large
                return BadRequest($"Update request too large. Maximum size of updates is 1000 current size is {tools.Count()}.");
            }
            else if (tools.Count() < 1)
            {
                return BadRequest("No tools were provided.");
            }

            await _repository.InsertBulkAsync(tools);
            return CreatedAtAction(nameof(GetToolsById), new { ids = tools.Select(tool => tool.Id) }, tools);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Tool>>> UpdateTool(int id, Tool tool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != tool.Id)
            {
                return BadRequest("Id mismatch.");
            }
            
            if (tool.Id <= 0)
            {
                return BadRequest("No valid id was provided.");
            }

            await _repository.UpdateOneAsync(tool);

            return Ok(tool);
        }

        [HttpPut("bulk")]
        public async Task<ActionResult<IEnumerable<Tool>>> UpdateToolBulk(IEnumerable<Tool> tools)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (tools.Select(tool => tool.Id).Contains(0))
            {
                return BadRequest();
            }

            if (tools.Count() > 1000)
            { // Don't want to make requests too large
                return BadRequest($"Update request too large. Maximum size of updates is 1000 current size is {tools.Count()}.");
            }
            else if (tools.Count() < 1)
            {
                return BadRequest("No valid ids were provided.");
            }

            await _repository.UpdateBulkAsync(tools);

            return Ok(tools);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTool(int id)
        {
            if (id <= 0)
            {
                return BadRequest("No valid ids were provided.");
            }

            await _repository.DeleteOneByIdAsync(id);
            return NoContent();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteToolBulk(IEnumerable<int> ids)
        {
            // The id 0 doesn't do anything and the database query shouldn't execute when no valid id is provided
            ids = ids.Where(id => id > 0).ToList();
            
            
            if (ids.Count() > 1000)
            { // Don't want to make requests too large
                return BadRequest($"Delete request too large. Maximum size of deletes is 1000 current size is { ids.Count() }.");
            }
            else if (ids.Count() < 1)
            {
                return BadRequest("No valid ids were provided.");
            }

            await _repository.DeleteBulkByIdAsync(ids);
            return NoContent();
        }
    }
}
