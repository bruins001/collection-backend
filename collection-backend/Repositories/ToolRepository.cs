using collection_backend.Data;
using collection_backend.Models;
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
