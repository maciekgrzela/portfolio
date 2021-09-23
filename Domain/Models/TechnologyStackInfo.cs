using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TechnologyStackInfo : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<TechnologyStackItemAndInfo> TechnologyStackItemAndInfos { get; set; } 
        public virtual Repo Repo { get; set; }
        public Guid RepoId { get; set; }
    }
}