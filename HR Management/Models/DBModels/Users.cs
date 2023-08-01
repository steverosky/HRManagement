namespace HR_Management.Models.DBModels
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime AddedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
