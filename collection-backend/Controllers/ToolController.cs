﻿using collection_backend.Models;
using collection_backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

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
            await _repository.InsertOneAsync(tool);
            return CreatedAtAction(nameof(GetToolsById), new { ids = tool.Id }, tool);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<Tool>>> InsertToolBulk(IEnumerable<Tool> tools)
        {
            await _repository.InsertBulkAsync(tools);
            return CreatedAtAction(nameof(GetToolsById), new { ids = tools.Select(tool => tool.Id) }, tools);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Tool>>> UpdateTool(int id, Tool tool)
        {
            if (tool.Id == 0)
            {
                return BadRequest();
            }

            await _repository.UpdateOneAsync(tool);

            return Ok(tool);
        }

        [HttpPut("bulk")]
        public async Task<ActionResult<IEnumerable<Tool>>> UpdateToolBulk(IEnumerable<Tool> tools)
        {
            if (tools.Select(tool => tool.Id).Contains(0))
            {
                return BadRequest();
            }

            await _repository.UpdateBulkAsync(tools);

            return Ok(tools);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTool(int id)
        {
            await _repository.DeleteOneByIdAsync(id);
            return NoContent();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteToolBulk(int[] ids)
        {
            await _repository.DeleteBulkByIdAsync(ids);
            return NoContent();
        }
    }
}
