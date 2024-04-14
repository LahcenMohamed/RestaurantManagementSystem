using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories.IReposioriesHelpers
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        public Task<Customer> GetByNameAsync(string FullName);
        public int CountOfLoyal();
    }
}
