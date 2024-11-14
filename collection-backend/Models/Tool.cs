namespace collection_backend.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string Type { get; set; } = null;
        public bool Electric {  get; set; }
        public string? ProductCode { get; set; } = null;
        public string? EAN { get; set; } = null;
    }
}
