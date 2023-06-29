﻿using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR_Management.Services
{
    public class DBServices : IDBServices
    {
        private readonly DataContext _context;
        public IConfiguration _config;
        ///private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public DBServices(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _context = context;
            _config = configuration;
            //_httpContextAccessor = httpContextAccessor;
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
        public string CreateToken(UserLogin user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtConfig:Secret").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                 _config["JwtConfig:Issuer"],
                _config["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task AddUser(AddEmployeeModel user)
        {
            //check if email exists
            var IsEmail = await _context.Employees.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (IsEmail is not null)
            {
                throw new Exception("User Already Exists");
            }
            
            EmployeeModel dbTable = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Nationality = user.Nationality,
                DateOfBirth = user.DateOfBirth,
                CreatedOn = DateTime.UtcNow
            };
            await _users.InsertOneAsync(dbTable);

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