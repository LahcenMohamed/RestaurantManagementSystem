using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;

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
            string query = $"INSERT INTO Orders (OrderDateTime,Price, DishId, CustomerId) " +
                           $"VALUES ('{model.OrderDateTime.Year}-{model.OrderDateTime.Month}-{model.OrderDateTime.Day}', " +
                           $"{model.Price} ," +
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
            var sql = @"
                        SELECT
                            o.Id, o.CustomerId, o.DishId, o.OrderDateTime, o.Price,
                            c.FullName,
                            d.Name
                        FROM Orders o
                        JOIN Customers c ON o.CustomerId = c.Id
                        JOIN Dishes d ON o.DishId = d.Id
                        ";

            return _context.Orders
                .FromSqlRaw(sql)
                .Include(o => o.Customer)
                .Include(o => o.Dish)
                .AsAsyncEnumerable();
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

        public int Count()
        {
            return _context.Orders.Count();
        }

        public int CountOfDay()
        {
            return _context.Orders.Count(x => x.OrderDateTime.Day == DateTime.Now.Day);
        }

        public async Task<Dictionary<string, int>> MaxDishes()
        {
            return _context.Orders.Include(d => d.Dish).AsEnumerable().GroupBy(x => x.Dish.Name).Select(x => new KeyValuePair<string, int>(x.Key, x.Count())).OrderByDescending(x => x.Value).Take(5).ToDictionary();
        }
    }
}
