using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;

namespace RestaurantManagementSystem.Repositories
{
    public sealed class DishRepository : IDishRepository
    {
        private readonly RestaurantManagementSystemDbContext _context;

        public DishRepository()
        {
            _context = new RestaurantManagementSystemDbContext();
        }

        public int Count()
        {
            return _context.Dishes.Count();
        }

        public int CountOfCommon()
        {
            return _context.Dishes.Count(x => x.isCommon);
        }

        public async Task CreateAsync(Dish model)
        {
            string query = $"INSERT INTO Dishes (Name, Description, Price,ImageUrl) " +
                           $"VALUES ('{model.Name.Replace("'", "''")}', " +
                           $"'{model.Description?.Replace("'", "''")}', " +
                           $"{model.Price}, " +
                           $"'{model.ImageUrl}')";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            string query = $"DELETE FROM Dishes WHERE Id = {id}";
            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }

        public IAsyncEnumerable<Dish> GetAllAsync()
        {
            return _context.Dishes.FromSqlRaw($"Select *from Dishes").AsAsyncEnumerable();
        }

        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _context.Dishes.FromSqlRaw($"Select *from Dishes where Id = {id}").FirstOrDefaultAsync();
        }

        public async Task<Dish> GetByNameAsync(string Name)
        {
            return await _context.Dishes.FromSqlRaw($"Select *from Dishes where Name = '{Name}'").FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<Dish> Search(string item)
        {
            decimal price = 0;
            decimal.TryParse(item, out price);
            return _context.Dishes.FromSqlRaw($"Select *from Dishes where Name like '%{item}%' or price = {price}").AsAsyncEnumerable();
        }

        public async Task UpdateAsync(Dish model)
        {
            string query = $"UPDATE Dishes " +
                           $"SET Name = '{model.Name.Replace("'", "''")}', " +
                           $"Description = '{model.Description?.Replace("'", "''")}', " +
                           $"Price = {model.Price}, " +
                           $"ImageUrl = '{model.ImageUrl?.Replace("'", "''")}' " +
                           $"WHERE Id = {model.Id}";

            _context.Database.ExecuteSqlRaw(query);
            await _context.SaveChangesAsync();
        }


    }
}
