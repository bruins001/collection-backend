using collection_backend.Data.QueryParameters;
using collection_backend.Models;

namespace collection_backend.Data.Filters;

public class ToolFilter
{
    public static async Task<IQueryable<Tool>> FilterAsync(IQueryable<Tool> query, ToolQueryParameters parameters)
    {
        // Else it is not allowed to be an async function
        query = await Task.Run(() =>
        {
            if (parameters.Id != null)
            {
                query = query.Where(tool => tool.Id == parameters.Id);
            }

            if (parameters.Name != null)
            {
                query = query.Where(tool => tool.Name.Contains(parameters.Name));
            }

            if (parameters.Description != null)
            {
                query = query.Where(tool => tool.Description != null ? tool.Description.Contains(parameters.Description) : false);
            }

            if (parameters.Type != null)
            {
                query = query.Where(tool => tool.Type.Contains(parameters.Type));
            }

            if (parameters.ProductCode != null)
            {
                query = query.Where(tool => tool.ProductCode != null ? tool.ProductCode.Contains(parameters.ProductCode) : false);
            }

            if (parameters.Ean != null)
            {
                query = query.Where(tool => tool.Ean != null ? tool.Ean.Contains(parameters.Ean) : false);
            }

            return query;
        });

        return query;
    }
}