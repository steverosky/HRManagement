using System.ComponentModel.DataAnnotations;

namespace HR_Management.Models.RequestModels
{
    public class AddEmployeeModel
    {
        public required string StaffId { get; set; } = string.Empty;
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string OtherName { get; set; } = string.Empty;
        public required Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.Date;
        [EmailAddress]
        public required string Email { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public bool SpecialNeed { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum MaritalStatus
    {
        Married,
        Divorced,
        Single,
        Separated,
        Widowed,
        Others
    }
}

