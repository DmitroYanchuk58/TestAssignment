using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTier.Entities
{
    public class Task:Entity
    {
        [Required]
        public string Title {  get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]  
        public Priority Priority { get; set; }

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
