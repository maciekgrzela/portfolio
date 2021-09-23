using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Repo : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MembersCount { get; set; }
        public string MyRoleInTeam { get; set; }
        public string Type { get; set; }
        public DateTime DoneDate { get; set; }
        public virtual List<TechnologyStackInfo> TechnologyStackInfos { get; set; }
        public virtual List<Link> Links { get; set; }
    }
}