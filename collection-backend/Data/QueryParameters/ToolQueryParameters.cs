namespace collection_backend.Data.QueryParameters;

public class ToolQueryParameters: QueryParameters
{
    // Hide properties
    public bool HideId { get; set; } = true;
    public bool HideName { get; set; } = true;
    public bool HideDescription { get; set; } = true;
    public bool HideType { get; set; } = true;
    public bool HideElectric { get; set; } = true;
    public bool HideProductCode { get; set; } = true;
    public bool HideEan { get; set; } = true;
    
    // Custom properties
    public int? Id { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public int? Type { get; set; } = null;
    public string? ProductCode { get; set; } = null;
    public string? Ean { get; set; } = null;
}