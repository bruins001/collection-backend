using collection_backend.Data;
using collection_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collection_backend.Repositories
{
    public class ToolRepository : IGenericRespository<Tool>
    {
        private readonly AppDbContext _context;

        public ToolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteBulkByIdAsync(int[] id)
        {
            IEnumerable<Tool>? tools = await _context.Tools.Where(tool => id.Contains(tool.Id)).ToListAsync();
            _context.RemoveRange(tools);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOneByIdAsync(int id)
        {
            Tool? tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                throw new KeyNotFoundException($"Could not find tool with id: {id}.");
            }
            _context.Remove(tool);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }

        public async Task<object> GetToolPageAsync(int page = 1, int size = 20, string orderBy = "")
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
            {
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

            switch (orderBy.ToLower())
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
            return await _context.Tools.Where(tool => ids.Contains(tool.Id)).ToListAsync();
        }

        public async Task<Tool?> GetOneByIdAsync(int id)
        {
            return await _context.Tools.FindAsync(id);
        }

        public async Task<IEnumerable<Tool>> InsertBulkAsync(IEnumerable<Tool> tools)
        {
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
