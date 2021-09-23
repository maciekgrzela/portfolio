using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Link : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Protected { get; set; }
        public string Value { get; set; }
        public virtual Repo Repo { get; set; }
        public Guid RepoId { get; set; }
    }
}