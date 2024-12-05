using DataTier.Entities;

namespace ApplicationTier.Models
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Status Status { get; set; } = Status.Pending;

        public Priority Priority { get; set; } = Priority.Low;


    }
}
