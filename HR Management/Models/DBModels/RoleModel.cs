namespace HR_Management.Models.DBModels
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public List<Permissions>? Permissions { get;set; }

    }
}
