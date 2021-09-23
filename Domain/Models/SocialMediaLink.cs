using System;

namespace Domain.Models
{
    public class SocialMediaLink : BaseEntity
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string Value { get; set; }
        public string Displayed { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}