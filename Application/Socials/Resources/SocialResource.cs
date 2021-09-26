using System;

namespace Application.Socials.Resources
{
    public class SocialResource
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string DisplayedName { get; set; }
        public string Path { get; set; }
    }
}