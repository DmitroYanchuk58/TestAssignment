using ApplicationTier.Services;
using DataTier.Entities;

namespace PresentationTier.DTO
{
    public class Filter
    {
        public string FilterItem;

        public Status? Status;

        public Priority? Priority;

        public DateTime? DueDate;

        public SortOption? SortOption;
    }
}
