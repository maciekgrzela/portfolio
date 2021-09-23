using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<SocialMediaLink> SocialMediaLinks { get; set; }
    }
}