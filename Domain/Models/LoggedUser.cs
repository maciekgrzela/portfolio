using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [NotMapped]
    public class LoggedUser : User
    {
        public string Token { get; set; }
    }
}