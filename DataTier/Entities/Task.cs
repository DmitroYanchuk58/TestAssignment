using System.ComponentModel.DataAnnotations;

namespace DataTier.Entities
{
    public class Task:Entity
    {
        [Required]
        public string Title {  get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Pending;

        [Required]  
        public Priority Priority { get; set; } = Priority.Low;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        public Guid IdUser { get; set; }

        public User User { get; set; }  
    }

    public enum Status
    {
        Pending,
        InProgress,
        Completed
    }
    public enum Priority
    {
        Low, 
        Medium, 
        High
    }
}
