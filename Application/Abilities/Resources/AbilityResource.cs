using System;
using Application.User.Resources;

namespace Application.Abilities.Resources
{
    public class AbilityResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double PercentageValue { get; set; }
        public UserForAbilityResource User { get; set; }
    }

    public class UserForAbilityResource : BasicUserResource { }
}