using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories.IReposioriesHelpers
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public IAsyncEnumerable<Order> GetByCustomerIdAsync(int customerId);
        public IAsyncEnumerable<Order> GetByDishIdAsync(int DishId);
    }
}
