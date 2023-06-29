
namespace HR_Management.Models.DBModels
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }

    }
}