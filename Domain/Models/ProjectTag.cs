using System;

namespace Domain.Models
{
    public class ProjectTag
    {
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}