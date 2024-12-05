using DataTier.Entities;

namespace PresentationTier.DTO
{
    public class TaskForUpdate
    {
        public Guid Id{get;set;}

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
