using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLadiesCorner.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Photo Name is required.")]
        [Display(Name = "Photo Name")] // This is for display in your UI
        public string Name { get; set; }

        [Display(Name = "Current Photo Path")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Upload New Photo File")]
        public IFormFile? ImageFile { get; set; }
        [StringLength(50)] // Made consistent with SKU, Color, Size for shorter text/categories
        public string Category { get; set; }

        [StringLength(50)] // Consistent length for colors
        public string Color { get; set; }

        [StringLength(50)] // Consistent length for sizes
        public string Size { get; set; }

        public bool IsActive { get; set; } = true;
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [StringLength(50)] // Consistent length for SKUs
        public string SKU { get; set; }

        [Required]
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default value, but can be manually set if controller allows
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
