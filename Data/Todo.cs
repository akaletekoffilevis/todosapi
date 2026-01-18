using System.ComponentModel.DataAnnotations;

namespace TodosApi.Data;

public class Todo
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
    public required string Title { get; set; }
    
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public required string Description { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key for User relationship
    [Required]
    public int UserId { get; set; }
    
    // Navigation property
    public User? User { get; set; }
}
