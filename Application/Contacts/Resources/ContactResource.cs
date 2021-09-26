using System;

namespace Application.Contacts.Resources
{
    public class ContactResource
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
    }
}