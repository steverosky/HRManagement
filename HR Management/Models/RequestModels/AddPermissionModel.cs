namespace HR_Management.Models.RequestModels
{
    public class AddPermissionModel
    {
        public int PermissionId { get; set; }
        public required string PermissionName { get; set; }
        public required string PermissionDescription { get; set; }
    }
}