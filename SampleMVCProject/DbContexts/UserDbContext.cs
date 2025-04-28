using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using SampleMVCProject.Models;


namespace SampleMVCProject.Repositories
{
    public class IdentityUser
    {
        private readonly IDbConnection _connectionString;

        public IdentityUser(IDbConnection connectionString)
        {
            _connectionString = connectionString;
        }

        // Example of creating a user with Dapper
        public async Task<int> CreateUserAsync(Users user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserName", user.UserName);
            parameters.Add("NormalizedUserName", user.NormalizedUserName);
            parameters.Add("Email", user.Email);
            parameters.Add("Password", user.Password);
            parameters.Add("NormalizedEmail", user.NormalizedEmail);
            parameters.Add("EmailConfirmed", user.EmailConfirmed);
            parameters.Add("SecurityStamp", user.SecurityStamp);
            parameters.Add("ConcurrencyStamp", user.ConcurrencyStamp);
            parameters.Add("PhoneNumber", user.PhoneNumber);
            parameters.Add("PhoneNumberConfirmed", user.PhoneNumberConfirmed);
            parameters.Add("TwoFactorEnabled", user.TwoFactorEnabled);
            parameters.Add("LockoutEnd", user.LockoutEnd);
            parameters.Add("LockoutEnabled", user.LockoutEnabled);
            parameters.Add("AccessFailedCount", user.AccessFailedCount);
            parameters.Add("Gender", user.Gender);
            parameters.Add("Department", user.Department);
            parameters.Add("ProfileImagePath", user.ProfileImagePath);

            return await _connectionString.ExecuteAsync(
                "InsertUser",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        // Example of getting a user by ID with Dapper
        public async Task<Users> GetUserByIdAsync(string userId)
        {
            var sql = "SELECT * FROM AspNetUsers WHERE Id = @UserId";
            return await _connectionString.QueryFirstOrDefaultAsync<Users>(sql, new { UserId = userId });
        }
    }
}
