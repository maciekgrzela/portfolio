using System;
using Application.User.Resources;

namespace Application.Achievements.Resources
{
    public class AchievementResource
    {
        public Guid Id { get; set; }
        public string Years { get; set; }
        public string Title { get; set; }
        public UserForAchievementResource User { get; set; }
    }

    public class UserForAchievementResource : BasicUserResource { }
}