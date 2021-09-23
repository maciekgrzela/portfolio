using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class TechnologyStackItem : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public virtual List<TechnologyStackItemAndInfo> TechnologyStackItemAndInfos { get; set; }
    }
}