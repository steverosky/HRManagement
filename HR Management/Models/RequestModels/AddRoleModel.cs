namespace HR_Management.Models.RequestModels
{
    public class AddRoleModel
    {
        //public string? RoleId { get; set; }
        public required string RoleName { get; set; }
        public required string RoleDescription { get; set; }
       // public List<Permissions>? Permissions { get; set; }
    }
}
