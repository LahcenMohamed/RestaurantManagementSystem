using GalaSoft.MvvmLight.CommandWpf;
using LiveCharts;
using LiveCharts.Defaults;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ObservableCollection<string> DishsNames { get; set; }
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

        public ObservableCollection<ChartValues<ObservableValue>> DishsValues { get; set; }

        public ICommand GetAllCommand { get; }

        public DashboardViewModel()
        {
            _customerRepository = new CustomerRepository();
            _orderRepository = new OrderRepository();
            _dishRepository = new DishRepository();
            DishsNames = new ObservableCollection<string>();
            DishsValues = new ObservableCollection<ChartValues<ObservableValue>>();
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
            DishsNames.Clear();
            var dishes = await _orderRepository.MaxDishes();
            for (int i = 0; i < 5; i++)
            {
                if (i < dishes.Count)
                {
                    DishsValues.Add(new ChartValues<ObservableValue>() { new ObservableValue(dishes.ElementAt(i).Value) });
                    DishsNames.Add(dishes.ElementAt(i).Key);
                }
                else
                {
                    DishsValues.Add(new ChartValues<ObservableValue>() { new ObservableValue(0) });
                    DishsNames.Add(null);
                }
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
