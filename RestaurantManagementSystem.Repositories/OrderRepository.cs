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
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly RestaurantManagementSystemDbContext _context;

        public OrderRepository()
        {
            _context = new RestaurantManagementSystemDbContext();
        }

        public async Task CreateAsync(Order model)
        {
            string query = $"INSERT INTO Orders (OrderDateTime, DishId, CustomerId) " +
                           $"VALUES ('{model.OrderDateTime}', " +
                           $"{model.DishId}, " +
                           $"{model.CustomerId})";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            string query = $"DELETE FROM Orders WHERE Id = {id}";
            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }

        public IAsyncEnumerable<Order> GetAllAsync()
        {
            return _context.Database.SqlQuery<Order>($"SELECT * FROM Orders").AsAsyncEnumerable();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Database.SqlQuery<Order>($"SELECT * FROM Orders WHERE Id = {id}").FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<Order> GetByCustomerIdAsync(int customerId)
        {
            return _context.Database.SqlQuery<Order>($"SELECT * FROM Orders WHERE CustomerId = {customerId}").AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Order> GetByDishIdAsync(int dishId)
        {
            return _context.Database.SqlQuery<Order>($"SELECT * FROM Orders WHERE DishId = {dishId}").AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Order> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return _context.Database.SqlQuery<Order>($"SELECT * FROM Orders WHERE OrderDateTime BETWEEN '{startDate}' AND '{endDate}'").AsAsyncEnumerable();
        }

        public async Task UpdateAsync(Order model)
        {
            string query = $"UPDATE Orders " +
                           $"SET OrderDateTime = '{model.OrderDateTime}', " +
                           $"DishId = {model.DishId}, " +
                           $"CustomerId = {model.CustomerId} " +
                           $"WHERE Id = {model.Id}";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }

        public IAsyncEnumerable<Order> Search(string item)
        {
            throw new NotImplementedException();
        }

        
    }
}
