using System.ComponentModel.DataAnnotations;

namespace DataTier.Entities
{
    public class User:Entity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(10)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } 

        public List<Task>? Tasks { get; set; }
    }
}
