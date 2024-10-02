namespace collection_backend.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Electric {  get; set; }
        public string ProductCode { get; set; }
        public int EAN { get; set; }
    }
}
