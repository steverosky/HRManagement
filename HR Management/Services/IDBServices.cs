using HR_Management.Models.RequestModels;

namespace HR_Management.Services
{
    public interface IDBServices
    {
        public Task AssignPermissionToRole(string permissionName, string roleName);
        public Task<Permissions> CreatePermission(AddPermissionModel permission);
        public Task<RoleModel> CreateRole(AddRoleModel role);
        public Task<List<EmployeeModel>> GetAllEmp();
        public Task<object> Login(UserLogin user);
        public  Task AddUser(AddEmployeeModel user);
        public Task<EmployeeModel> GetUser(string email);
    }
}
