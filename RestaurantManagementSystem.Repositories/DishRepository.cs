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
    public sealed class DishRepository : IDishRepository
    {
        private readonly RestaurantManagementSystemDbContext _context;

        public DishRepository()
        {
            _context = new RestaurantManagementSystemDbContext();
        }

        public async Task CreateAsync(Dish model)
        {
            string query = $"INSERT INTO Dishes (Name, Description, Price) " +
                           $"VALUES ('{model.Name.Replace("'", "''")}', " +
                           $"'{model.Description?.Replace("'", "''")}', " +
                           $"{model.Price})";

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
            return _context.Database.SqlQuery<Dish>($"Select *from Dishs").AsAsyncEnumerable();
        }

        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _context.Database.SqlQuery<Dish>($"Select *from Dishs where Id = {id}").FirstOrDefaultAsync();
        }

        public async Task<Dish> GetByNameAsync(string Name)
        {
            return await _context.Database.SqlQuery<Dish>($"Select *from Dishs where Name = '{Name}'").FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<Dish> Search(string item)
        {
            return _context.Database.SqlQuery<Dish>($"Select *from Dishs where Id = {item} or Name like '%{item}%' or price = {item}").AsAsyncEnumerable();
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
