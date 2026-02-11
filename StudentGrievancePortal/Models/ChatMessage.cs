using System.ComponentModel.DataAnnotations;

namespace StudentGrievancePortal.Models;

public class ChatMessage
{
    [Key]
    public int MessageId { get; set; }
    
    public int? StudentId { get; set; }
    
    [Required]
    public string SessionId { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = string.Empty; // "user" or "assistant"
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}