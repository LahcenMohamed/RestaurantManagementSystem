using GalaSoft.MvvmLight.CommandWpf;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantManagementSystem.ViewModels
{
    public sealed class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly IDishRepository _dishRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private int countOfCustomers;
        private int countOfDishs;
        private int countOfOrders;

        private int countOfLoyalCustomers;
        private int countOfCommonDishs;
        private int countOfDayOrders;

        public int CountOfCustomers
        {
            get
            {
                return countOfCustomers;
            }
            set
            {
                countOfCustomers = value;
                onPropertyChanged(nameof(CountOfCustomers));
            }
        }

        public int CountOfDishs
        {
            get
            {
                return countOfDishs;
            }
            set
            {
                countOfDishs = value;
                onPropertyChanged(nameof(CountOfDishs));
            }
        }

        public int CountOfOrders
        {
            get
            {
                return countOfOrders;
            }
            set
            {
                countOfOrders = value;
                onPropertyChanged(nameof(CountOfOrders));
            }
        }

        public int CountOfLoyalCustomers
        {
            get
            {
                return countOfLoyalCustomers;
            }
            set
            {
                countOfLoyalCustomers = value;
                onPropertyChanged(nameof(CountOfLoyalCustomers));
            }
        }

        public int CountOfCommonDishs
        {
            get
            {
                return countOfCommonDishs;
            }
            set
            {
                countOfCommonDishs = value;
                onPropertyChanged(nameof(CountOfCommonDishs));
            }
        }

        public int CountOfDayOrders
        {
            get
            {
                return countOfDayOrders;
            }
            set
            {
                countOfDayOrders = value;
                onPropertyChanged(nameof(CountOfDayOrders));
            }
        }



        public ICommand GetAllCommand { get; }

        public DashboardViewModel()
        {
            _customerRepository = new CustomerRepository();
            _orderRepository = new OrderRepository();
            _dishRepository = new DishRepository();
            GetAllCommand = new RelayCommand(GetAll);
        }

        private async void GetAll()
        {
            CountOfCustomers = _customerRepository.Count();
            CountOfDishs = _dishRepository.Count();
            CountOfOrders = _orderRepository.Count();
            CountOfCommonDishs = _dishRepository.CountOfCommon();
            CountOfDayOrders = _orderRepository.CountOfDay();
            CountOfLoyalCustomers = _customerRepository.CountOfLoyal();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
