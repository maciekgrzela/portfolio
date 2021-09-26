using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Achievement : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Years { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}