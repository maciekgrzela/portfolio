using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ContactFormRequest
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string Email { get; set; }
        [Required, MaxLength(3000)]
        public string Content { get; set; }
        [Required]
        public DateTime Sent { get; set; } = DateTime.Now;
    }
}