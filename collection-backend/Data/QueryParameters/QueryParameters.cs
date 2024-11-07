namespace collection_backend.Data.QueryParameters;

public class QueryParameters
{
    const int MaxPageSize = 100;
    public int Page { get; set; } = 1;
    public int Limit
    {
        get { return Limit; }
        set { Limit = (value > MaxPageSize) ? MaxPageSize : value; }
    }
}