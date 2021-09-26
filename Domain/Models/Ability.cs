using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Ability : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, Range(0, 100)]
        public double PercentageValue { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}