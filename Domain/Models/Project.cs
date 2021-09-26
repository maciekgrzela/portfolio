using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Project : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int WorkGroup { get; set; }
        [Required, MaxLength(100)]
        public string Responsibility { get; set; }
        [Required]
        public virtual List<ProjectTag> ProjectTags { get; set; }
        [Required, MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public virtual RepositoryLink RepositoryLink { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        public virtual List<ProjectImage> ProjectImages { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }

    public class ProjectImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public virtual Project Project { get; set; }
        public Guid ProjectId { get; set; }
    }
}