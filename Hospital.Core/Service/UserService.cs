using Dapper;
using Hospital.Model.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Hospital.Core.Service
{    

    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));


        
        public async Task<bool> RegisterUser(RegisterDto request)
        {
            using var connection = CreateConnection();

            
            string checkSql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            int count = await connection.ExecuteScalarAsync<int>(checkSql, new { request.Email });

            if (count > 0)
                return false; 

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            
            string insertSql = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES (@Username, @Email, @PasswordHash, 'User')";
            await connection.ExecuteAsync(insertSql, new { request.Username, request.Email, PasswordHash = hashedPassword });

            return true;
        }

        
        public async Task<string> LoginUser(LoginDto request)
        {
            using var connection = CreateConnection();

            
            string sql = "SELECT * FROM Users WHERE Email = @Email";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { request.Email });

            var result = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null; 

            return GenerateJwtToken(user);
        }

        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = CreateConnection();
            string sql = "SELECT Id, Username, Role FROM Users";
            return await connection.QueryAsync<User>(sql);
        }

        
        public async Task AssignRoleToUser(int userId, string roleName)
        {
            using var connection = CreateConnection();
            string sql = "UPDATE Users SET Role = @RoleName WHERE Id = @UserId";
            await connection.ExecuteAsync(sql, new { UserId = userId, RoleName = roleName });
        }

        
        public async Task AssignPermissionToUser(int userId, string permission)
        {
            using var connection = CreateConnection();
            string sql = "INSERT INTO UserPermissions (UserId, Permission) VALUES (@UserId, @Permission)";
            await connection.ExecuteAsync(sql, new { UserId = userId, Permission = permission });
        }

        
        public async Task<bool> HasPermission(int userId, string permission)
        {
            using var connection = CreateConnection();
            string sql = "SELECT COUNT(1) FROM UserPermissions WHERE UserId = @UserId AND Permission = @Permission";
            return await connection.ExecuteScalarAsync<bool>(sql, new { UserId = userId, Permission = permission });
        }
       
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
