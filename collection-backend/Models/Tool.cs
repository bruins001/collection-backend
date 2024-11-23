using System.ComponentModel.DataAnnotations;

namespace collection_backend.Models
{
    public class Tool
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        
        [Display(Name = "description")]
        public string? Description { get; set; } = null;
        
        [Display(Name = "type")]
        [Required]
        [MaxLength(150)]
        public string Type { get; set; }
        
        [Display(Name = "electric")]
        public bool Electric {  get; set; } = true;
        
        [Display(Name = "prodctCode")]
        [MaxLength(50)]
        public string? ProductCode { get; set; } = null;
        
        [Display(Name = "ean")]
        [Length(13, 13)]
        public string? Ean { get; set; } = null;
    }
}
