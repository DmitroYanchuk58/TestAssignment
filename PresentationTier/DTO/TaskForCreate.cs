using DataTier.Entities;
using System.ComponentModel.DataAnnotations;

namespace PresentationTier.DTO
{
    public class TaskForCreate
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Status Status { get; set; } = Status.Pending;

        public Priority Priority { get; set; } = Priority.Low;

    }
}
