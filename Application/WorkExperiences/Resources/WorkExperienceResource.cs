using System;
using Application.User.Resources;

namespace Application.WorkExperiences.Resources
{
    public class WorkExperienceResource
    {
        public Guid Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
        public UserForWorkExperienceResource User { get; set; }
    }

    public class UserForWorkExperienceResource : BasicUserResource { }
}