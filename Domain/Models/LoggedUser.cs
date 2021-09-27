using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [NotMapped]
    public class LoggedUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}