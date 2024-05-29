using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using RestaurantManagementSystem.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManagementSystem.ViewModels
{
    public sealed class OrderViewModel : INotifyPropertyChanged
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDishRepository _dishRepository;
        private string searchItem;
        private string customerName;
        private string dishName;
        private AddEditOrder addEditOrder;
        private decimal price;

        public Order Order { get; set; }
        public Order SelectedOrder { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<string> CustomersNames { get; set; }
        public ObservableCollection<string> DishsNames { get; set; }
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                if (customerName != value)
                {
                    customerName = value;
                    ChangeCustomerName();
                    onPropertyChanged(nameof(customerName));

                }
            }
        }
        public string DishName
        {
            get
            {
                return dishName;
            }
            set
            {
                if (dishName != value)
                {
                    dishName = value;
                    ChangeDishName();
                    onPropertyChanged(nameof(dishName));

                }
            }
        }
        public string SearchItem
        {
            get
            {
                return searchItem;
            }
            set
            {
                if (searchItem != value)
                {
                    searchItem = value;
                    SearchAsync();
                    onPropertyChanged(nameof(searchItem));

                }
            }
        }
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                onPropertyChanged(nameof(Price));
            }
        }

        public ICommand AddCommand { get; }

        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand GetAllCommand { get; }
        public OrderViewModel()
        {
            _orderRepository = new OrderRepository();
            _customerRepository = new CustomerRepository();
            _dishRepository = new DishRepository();
            Order = new Order();
            Orders = new ObservableCollection<Order>();
            DishsNames = new();
            CustomersNames = new();
            searchItem = string.Empty;
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SaveCommand = new RelayCommand(Save);
            GetAllCommand = new RelayCommand(GetAll);
        }

        private async void ChangeCustomerName()
        {
            if (!customerName.IsNullOrEmpty())
            {
                var cust = await _customerRepository.GetByNameAsync(customerName);
                if (cust.IsLoyal)
                {
                    Price *= 0.90m;
                }
            }
        }
        private async void ChangeDishName()
        {
            if (!dishName.IsNullOrEmpty())
            {
                var dish = await _dishRepository.GetByNameAsync(dishName);
                Price = dish.Price;
                if (!customerName.IsNullOrEmpty())
                {
                    var cust = await _customerRepository.GetByNameAsync(customerName);
                    if (cust.IsLoyal)
                    {
                        Price *= 0.90m;
                    }
                }

            }
        }
        private async void SearchAsync()
        {
            Orders.Clear();
            await foreach (var order in _orderRepository.Search(searchItem))
            {
                Orders.Add(order);
            }
        }
        private async void GetAll()
        {
            if (!Orders.IsNullOrEmpty())
                return;
            await foreach (var item in _orderRepository.GetAllAsync())
            {
                Orders.Add(item);
            }
        }
        private async Task GetAllForOp()
        {
            Orders.Clear();
            await foreach (var item in _orderRepository.GetAllAsync())
            {
                Orders.Add(item);
            }
        }
        private async void Add()
        {
            SelectedOrder = null;
            Order = new Order();
            DishsNames.Clear();
            CustomersNames.Clear();

            await foreach (var item in _customerRepository.GetAllAsync())
            {
                CustomersNames.Add(item.FullName);
            }
            await foreach (var item in _dishRepository.GetAllAsync())
            {
                DishsNames.Add(item.Name);
            }
            dishName = customerName = string.Empty;
            Order.OrderDateTime = DateTime.Now;
            addEditOrder = new AddEditOrder(this);
            addEditOrder.Show();
        }

        public void Update()
        {
            Order = new Order();
            if (SelectedOrder is null)
            {
                MessageBox.Show("S’il vous plaît sélectionner un client");
                return;
            }
            Order = SelectedOrder;
            addEditOrder = new AddEditOrder(this);
            addEditOrder.Show();
        }

        public async void Save()
        {
            try
            {
                if (SelectedOrder is null)
                {
                    var cust = await _customerRepository.GetByNameAsync(customerName);
                    var dish = await _dishRepository.GetByNameAsync(dishName);
                    Order.DishId = dish.Id;
                    Order.CustomerId = cust.Id;
                    Order.Price = Price;
                    await _orderRepository.CreateAsync(Order);
                    MessageBox.Show("L’ajout de plat a été couronné de succès");

                }
                else
                {
                    await _orderRepository.UpdateAsync(Order);
                    MessageBox.Show("L’modifié de plat a été couronné de succès");
                }
                addEditOrder.Close();
                await GetAllForOp();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s’est produite lors de la validation des informations");
            }
        }

        public async void Delete()
        {
            try
            {
                if (SelectedOrder is null)
                {
                    MessageBox.Show("S’il vous plaît sélectionner un plat");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($" Voulez-vous supprimer le plat {SelectedOrder.Id} ? ", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _orderRepository.DeleteAsync(SelectedOrder.Id);
                    await GetAllForOp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur");
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
