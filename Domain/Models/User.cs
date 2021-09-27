using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(2000)]
        public string SelfDescription { get; set; }
        [Required]
        public bool LookingForAJob { get; set; }
        public virtual List<SocialMediaLink> SocialMediaLinks { get; set; }
        public virtual List<Achievement> Achievements { get; set; }
        public virtual List<WorkExperience> WorkExperiences { get; set; }
        public virtual List<Ability> Abilities { get; set; }
        public virtual List<Project> Projects { get; set; }
        public string GithubPath { get; set; }
        public string AzureDevOpsPath { get; set; }
        public string BitbucketPath { get; set; }
    }
}