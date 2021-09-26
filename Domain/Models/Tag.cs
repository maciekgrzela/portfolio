using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Value { get; set; }
        public virtual List<ProjectTag> ProjectTags { get; set; }
    }
}