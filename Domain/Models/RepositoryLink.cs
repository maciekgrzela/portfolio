using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RepositoryLink : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(30)]
        public string Platform { get; set; }
        [Required]
        public string Path { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}