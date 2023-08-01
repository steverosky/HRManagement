using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HR_Management.Services
{
    public class DBServices : IDBServices
    {
        private readonly DataContext _context;
        public IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public DBServices(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _context = context;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }

        //Hash password
        public static string CreatePasswordhash(string password)
        {
            if (password != null)
            {
                password = BCrypt.Net.BCrypt.HashPassword(password);

                return password;
            }
            else throw new Exception("invalid");

        }


        //Create Token for authentication
        public string CreateToken(string email, string Id)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtConfig:Secret").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                 _config["JwtConfig:Issuer"],
                _config["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append("token", jwt, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddSeconds(3000),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None

            });
            return jwt;
        }

        public async Task AddEmployee(AddEmployeeModel user)
        {
            try
            {
                //check if email exists
                var IsEmail = await _context.Employees.FirstOrDefaultAsync(p => p.Email == user.Email);
                if (IsEmail is not null)
                {
                    throw new Exception("Employee Already Exists");
                }
                var id = Guid.NewGuid();
                EmployeeModel dbTable = new()
                {
                    Id = id,
                    StaffId = user.StaffId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    OtherName = user.OtherName,
                    Email = user.Email,
                    Nationality = user.Nationality,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    PasswordHash = CreatePasswordhash(user.Password),
                    MaritalStatus = user.MaritalStatus,
                    Region = user.Region,
                    SpecialNeed = user.SpecialNeed,
                    Status = user.Status,
                    DeptId = user.DeptId,
                    RoleId = user.RoleId,
                    ManagerId = user.ManagerId,
                    IsDeleted = false,
                    AddedOn = DateTime.UtcNow

                };
                _context.Employees.Add(dbTable);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception( ex.Message);
            }

        }


        public async Task AddUser(UserLogin user)
        {
            try
            {
                //check if email exists
                var IsEmail = await _context.Users.FirstOrDefaultAsync(p => p.Email == user.Email);
                if (IsEmail is not null)
                {
                    throw new Exception("User Already Exists");
                }
                var id = Guid.NewGuid();
                Users dbTable = new()
                {
                    Id = id,
                    Email = user.Email,
                    PasswordHash = CreatePasswordhash(user.Password),
                    AddedOn = DateTime.UtcNow,
                    IsDeleted = false

                };
                _context.Users.Add(dbTable);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<object> Login(UserLogin user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email && e.IsDeleted == false);

            if (dbUser == null)
            {
                throw new Exception("Email or Password is Incorrect");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, dbUser.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Email or Password is Incorrect");
            }

            string token = CreateToken(dbUser.Email, dbUser.Id.ToString());

            return new TokenResponse
            {
                Email = dbUser.Email

            };
        }


        //get all employees
        public async Task<List<EmployeeModel>> GetAllEmp() => await _context.Employees.Where(a => a.IsDeleted == false).ToListAsync();

        //Get employee by email
        public async Task<EmployeeModel> GetEmployee(string email)
        {
             var emp = await _context.Employees.Where(e => e.Email == email && e.IsDeleted == false).FirstOrDefaultAsync();

            if (emp is null)
                throw new Exception("Employee not found");

            return emp;
            
        }


        //get all Users
        public async Task<List<Users>> GetAllUsers() => await _context.Users.Where(a => a.IsDeleted == false).ToListAsync();

        //Get user by email
        public async Task<Users> GetUser(string email)
        {
            var emp = await _context.Users.Where(e => e.Email == email && e.IsDeleted == false ).FirstOrDefaultAsync();

            if (emp is null)
                throw new Exception("Employee not found");

            return emp;


        }

        //create new role
        public async Task<RoleModel> CreateRole(AddRoleModel role)
        {
            var idList = await _context.Roles.Select(x => x.RoleId).ToListAsync();
            var roleId = idList.Any() ? idList.Max() + 1 : 1;
            RoleModel response = new()
            {
                RoleId = roleId,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
            };
            _context.Roles.Add(response);
            _context.SaveChanges();
            return response;
        }

        //create new Permission
        public async Task<Permissions> CreatePermission(AddPermissionModel permission)
        {
            var idList = await _context.Permissions.Select(x => x.PermissionId).ToListAsync();
            var permissionId = idList.Any() ? idList.Max() + 1 : 1;
            Permissions response = new()
            {
                PermissionId = permissionId,
                PermissionName = permission.PermissionName,
                PermissionDescription = permission.PermissionName
            };
            _context.Permissions.Add(response);
            _context.SaveChanges();
            return response;
        }

        public async Task AssignPermissionToRole(string permissionName, string roleName)
        {
            // Retrieve the role and permission objects based on the given names
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionName);

            if (role != null && permission != null)
            {
                // Add the permission to the role's list of permissions
                RoleModel roleModel = new()
                {
                    Permissions = new List<Permissions> {
                    new Permissions
                    {
                        PermissionId= permission.PermissionId,
                        PermissionName = permission.PermissionName,
                        PermissionDescription = permission.PermissionDescription
                    }}
                };
                // Save the changes to the role object, updating the role's permissions
                _context.Roles.Update(roleModel);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Role or permission not found");
            }
        }


        //Method to check if the specified role is valid and supported by the application
        private bool IsValidRole(string role)
        {
            // Check if the role is supported by the application and return the result
            return (role == "user" || role == "admin" || role == "moderator");
        }


        //Dictionary to map roles to their corresponding permissions
        private Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
        {
            { "user", new List<string>() { "ChangePass", "UpdateUser", "Login" } },
            { "admin", new List<string>() { "ChangePass", "UpdateUser", "Login", "AddUser", "DeleteUser", "GetUsers", "GetUserById", "AssignRoles" } }
        };

        // Method to check if the user has the specified permission
        private bool HasPermission(string role, string permission)
        {
            // Check if the role exists in the dictionary and if it has the specified permission
            if (rolePermissions.ContainsKey(role) && rolePermissions[role].Contains(permission))
            {
                return true;
            }
            return false;
        }


    }
}
