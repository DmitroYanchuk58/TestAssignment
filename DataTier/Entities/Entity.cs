using System.ComponentModel.DataAnnotations;

namespace DataTier.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}

