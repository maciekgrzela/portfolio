using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SocialMediaLink : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(30)]
        public string Platform { get; set; }
        [Required, MaxLength(50)]
        public string DisplayedName { get; set; }
        [Required]
        public string Path { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}