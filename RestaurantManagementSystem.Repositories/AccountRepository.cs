using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public sealed class AccountRepository
    {
        private readonly RestaurantManagementSystemDbContext _context;
        public AccountRepository()
        {
            _context = new RestaurantManagementSystemDbContext();
        }

        public async Task Register(User user)
        {
            string query = $"INSERT INTO Users (UserName, Password) " +
                           $"VALUES ('{user.UserName}', " +
                           $"{user.Password}) ";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Login(User user)
        {
            var checkuser = _context.Users
                        .FromSqlRaw($"SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password",
                            new SqlParameter("@UserName", user.UserName),
                            new SqlParameter("@Password", user.Password))
                        .ToList();

            return checkuser.Count() > 0;
        }
    }
}
