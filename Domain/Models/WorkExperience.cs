using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class WorkExperience : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}