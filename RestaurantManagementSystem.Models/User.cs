using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Models
{
    public sealed class User
    {
        public int Id { get; set; }
        [Required]
        [Length(3,50)]
        public string UserName { get; set; }
        [Required]
        [Length(2,11)]
        public string Password { get; set; }
    }
}
