using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantManagementSystemDbContext _context;
        public CustomerRepository()
        {
            _context = new RestaurantManagementSystemDbContext();
        }

        public int Count()
        {
            return _context.Customers.Count();
        }

        public int CountOfLoyal()
        {
            return _context.Customers.Count(x => x.IsLoyal);
        }

        public async Task CreateAsync(Customer model)
        {
            string query = $"INSERT INTO Customers (FullName, PhoneNumber, Address, IsLoyal) " +
                           $"VALUES ('{model.FullName.Replace("'", "''")}', " +
                           $"'{model.PhoneNumber.Replace("'", "''")}', " +
                           $"'{model.Address?.Replace("'", "''")}', " +
                           $"{(model.IsLoyal ? 1 : 0)})";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }


        public Task DeleteAsync(int Id)
        {
            string query = $"delete from Customers where Id = {Id}";
            _context.Database.ExecuteSqlRaw(query);
            return _context.SaveChangesAsync();
        }

        public IAsyncEnumerable<Customer> GetAllAsync()
        {
            var sql = "SELECT * FROM Customers";
            return _context.Customers.FromSqlRaw(sql).AsAsyncEnumerable();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FromSqlRaw($"Select *from Customers where Id = {id}").FirstOrDefaultAsync();
        }

        public async Task<Customer> GetByNameAsync(string FullName)
        {
            return await _context.Customers.FromSqlRaw($"Select *from Customers where FullName = '{FullName.Replace("'", "''")}'")
                                                            .FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<Customer> Search(string item)
        {
            return _context.Customers
                  .FromSqlRaw($"select *from Customers where FullName like '%{item.Replace("'", "''")}%' or PhoneNumber like '%{item.Replace("'", "''")}%' or Address like '%{item.Replace("'", "''")}%'")
                  .AsAsyncEnumerable();
        }

        public async Task UpdateAsync(Customer model)
        {
            string query = $"UPDATE Customers " +
               $"SET FullName = '{model.FullName.Replace("'", "''")}', " +
               $"PhoneNumber = '{model.PhoneNumber.Replace("'", "''")}', " +
               $"Address = '{model.Address?.Replace("'", "''")}', " +
               $"IsLoyal = {(model.IsLoyal ? 1 : 0)} " +
               $"WHERE Id = {model.Id}";
            ;

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }
    }
}
