using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Models.RequestModels
{
    public class UserLogin
    {
        [EmailAddress]
        public required string Email { get; set; }
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
