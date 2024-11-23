using collection_backend.Data;
using collection_backend.Data.Filters;
using collection_backend.Data.QueryParameters;
using collection_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collection_backend.Repositories
{
    public class ToolRepository : IToolRepository
    {
        private readonly AppDbContext _context;

        public ToolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteBulkByIdAsync(IEnumerable<int> ids)
        {
            ids = ids.Where(id => id > 0).ToList();
            
            if (ids.Count() > 1000)
            { // Don't want to make requests too large
                throw new BadHttpRequestException($"Maximum size of delete requests is 1000 current size is { ids.Count() }.");
            }
            else if (ids.Count() < 1)
            {
                throw new BadHttpRequestException("No valid ids were provided.");
            }

            IEnumerable<Tool>? tools = await _context.Tools.Where(tool => ids.Contains(tool.Id)).ToListAsync();
            _context.RemoveRange(tools);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }

        public async Task DeleteOneByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("No valid ids were provided.");
            }
            
            Tool? tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                throw new KeyNotFoundException($"Could not find tool with id: {id}.");
            }
            _context.Remove(tool);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tool>> GetAllAsync([FromQuery] ToolQueryParameters queryParameters)
        {
            IQueryable<Tool> query = await ToolFilter.FilterAsync(_context.Tools.AsQueryable(), queryParameters);
            return await query.ToListAsync();
        }

        public async Task<object> GetToolPageAsync(ToolQueryParameters queryParameters, int page = 1, int size = 20)
        {
            if (page <= 0)
            {
                throw new BadHttpRequestException($"Page number must be greater then 0 current page is {page}.");
            }

            if (size <= 0)
            {
                throw new BadHttpRequestException($"Page size must be greater then 0 current size is {size}.");
            }

            if (size > 1000)
            { // Don't want to make the requests too large
                throw new BadHttpRequestException($"Page size can't be greater then 1000 current size is {size}.");
            }

            int pageCount = _context.Tools.Count();
            double totalPages = Math.Ceiling((double) pageCount / size);

            if (page > totalPages)
            {
                throw new BadHttpRequestException($"Page {page} doesn't exist.");
            }

            int? nextPage = (page + 1) <= totalPages ? page + 1 : null;
            int? prevPage = (page - 1) >= 1 ? page - 1 : null;
            var query = _context.Tools.Distinct();

            switch (queryParameters.OrderBy?.ToLower())
            {
                case "name":
                    query = query.OrderBy(tool => tool.Name);
                    break;
                case "type":
                    query = query.OrderBy(tool => tool.Type);
                    break;
                case "electric":
                    query = query.OrderBy(tool => tool.Electric);
                    break;
                case "productcode":
                    query = query.OrderBy(tool => tool.ProductCode);
                    break;
                default:
                    query = query.OrderBy(tool => tool.Name);
                    break;
            }

            IEnumerable<Tool> tools = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            var pagenation = new { total_records = pageCount, current_page = page,
                total_pages = totalPages, next_page = nextPage, prev_page = prevPage };

            return new { data = tools, pagenation };
        }

        public async Task<IEnumerable<Tool>?> GetBulkByIdAsync(IEnumerable<int> ids)
        {
            if (ids.Count() > 1000)
            { // Don't want to make requests too large
                throw new BadHttpRequestException($"Maximum size of get requests is 1000 current size is { ids.Count() }.");
            }
            else if (ids.Count() < 1)
            {
                throw new BadHttpRequestException("No valid ids were provided.");
            }

            IEnumerable<Tool> tools = await _context.Tools.Where(tool => ids.Contains(tool.Id)).ToListAsync();
            if (tools.Count() < 1)
            {
                throw new KeyNotFoundException($"Couldn't find any items from ids: {ids}.");
            }

            return await _context.Tools.Where(tool => ids.Contains(tool.Id)).ToListAsync();
        }

        public async Task<Tool?> GetOneByIdAsync(int id)
        {
            return await _context.Tools.FindAsync(id);
        }

        public async Task<IEnumerable<Tool>> InsertBulkAsync(IEnumerable<Tool> tools)
        {
            if (tools.Count() > 1000)
            { // Don't want to make requests too large
                throw new BadHttpRequestException($"Maximum size of insertions is 1000 current size is { tools.Count() }.");
            }
            else if (tools.Count() < 1)
            {
                throw new BadHttpRequestException("No tools were provided.");
            }

            await _context.Tools.AddRangeAsync(tools);
            await _context.SaveChangesAsync();
            return tools;
        }

        public async Task<Tool> InsertOneAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
            return tool;
        }

        public async Task<IEnumerable<Tool>> UpdateBulkAsync(IEnumerable<Tool> tools)
        {
            if (tools.Count() > 1000)
            { // Don't want to make requests too large
                throw new BadHttpRequestException($"Maximum size of updates is 1000 current size is { tools.Count() }.");
            }

            try
            {
                _context.Tools.UpdateRange(tools);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new KeyNotFoundException($"Some ids are not found as tool ids: { tools.Select(tool => tool.Id) }");
            }
            _context.Tools.UpdateRange(tools);
            await _context.SaveChangesAsync();
            return tools;
        }

        public async Task<Tool> UpdateOneAsync(Tool tool)
        {
            if (tool.Id <= 0)
            {
                throw new BadHttpRequestException("No valid id was provided.");
            }

            try
            {
                _context.Tools.Update(tool);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new KeyNotFoundException($"Tool with id: {tool.Id} doesn't exist in the database.");
            }

            return tool;
        }
    }
}
