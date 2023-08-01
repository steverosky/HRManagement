using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Models.RequestModels
{
    public class UserLogin
    {
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z.]{2,}$", ErrorMessage = "Valid Email is Required")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*#?&.])[A-Za-z\\d@$!%*#?&.]{10,}$", ErrorMessage = "Password Format Invalid")]
        public required string Password { get; set; }
        //[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
