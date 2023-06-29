namespace HR_Management.Models.DBModels
{
    public class Permissions
    {
        public Guid Id { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDescription { get; set; }
    }
}