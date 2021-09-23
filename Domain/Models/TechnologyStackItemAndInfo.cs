using System;

namespace Domain.Models
{
    public class TechnologyStackItemAndInfo : BaseEntity
    {
        public Guid TechnologyStackInfoId { get; set; }
        public virtual TechnologyStackInfo TechnologyStackInfo { get; set; }
        public Guid TechnologyStackItemId { get; set; }
        public virtual TechnologyStackItem TechnologyStackItem { get; set; }
    }
}