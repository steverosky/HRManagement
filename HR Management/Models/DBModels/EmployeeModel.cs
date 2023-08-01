using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace HR_Management.Models.DBModels
{
    [Table("Employees", Schema = "dbo")]
    public class EmployeeModel
    {
        [Key, Required]
        public Guid Id { get; set; }
        public string StaffId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; } 
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.Date;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash{ get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public Address? Address { get; set; }
        public bool SpecialNeed { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime AddedOn { get; set; }

        public string GetFullName()
        {
            return $"{this.LastName} {this.FirstName}";
        }
    }

    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Door { get; set; } = string.Empty;
        public string Street1 { get; set; } = string.Empty;
        public string Street2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
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
