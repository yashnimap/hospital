using Hospital.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{    
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterDto request);
        Task<string> LoginUser(LoginDto request);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AssignRoleToUser(int userId, string roleName);
        Task AssignPermissionToUser(int userId, string permission);
        Task<bool> HasPermission(int userId, string permission);
    }

}
