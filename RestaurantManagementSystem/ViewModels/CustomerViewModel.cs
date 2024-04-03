using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RestaurantManagementSystem.Repositories;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementSystem.Views;
using System.Windows;

namespace RestaurantManagementSystem.ViewModels
{
    public sealed class CustomerViewModel : INotifyPropertyChanged
    {
        private readonly ICustomerRepository _customerRepository;
        private string searchItem;
        private AddEditWindow addEditWindow;

        public Customer Customer { get; set; }
        public Customer SelectedCustomer { get; set; }
        public ObservableCollection<Customer> Customers { get; set; }
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

        public ICommand AddCommand { get; }

        public ICommand UpdateCommand  { get; }

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand GetAllCommand { get; }

        public CustomerViewModel()
        {
            _customerRepository = new CustomerRepository();
            Customer = new Customer();
            Customers = new ObservableCollection<Customer>();
            searchItem = string.Empty;
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SaveCommand = new RelayCommand(Save);
            GetAllCommand = new RelayCommand(GetAll);
            addEditWindow = new AddEditWindow(this);
        }

        private async void SearchAsync()
        {
            Customers.Clear();
            await foreach (var customer in _customerRepository.Search(searchItem))
            {
                Customers.Add(customer);
            }
        }
        private async void GetAll()
        {
            if (!Customers.IsNullOrEmpty())
                return;
            await foreach (var item in _customerRepository.GetAllAsync())
            {
                Customers.Add(item);
            }
        }
        private async Task GetAllForOp()
        {
            Customers.Clear();
            await foreach (var item in _customerRepository.GetAllAsync())
            {
                Customers.Add(item);
            }
        }
        private void Add()
        {
            SelectedCustomer = null;
            addEditWindow = new AddEditWindow(this);
            addEditWindow.Show();
        }

        public void Update()
        {
            if (SelectedCustomer is null)
            {
                MessageBox.Show("S’il vous plaît sélectionner un client");
                return;
            }
            Customer = SelectedCustomer;
            addEditWindow = new AddEditWindow(this);
            addEditWindow.Show();
        }

        public async void Save()
        {
            try
            {
                if (SelectedCustomer is null)
                {
                    await _customerRepository.CreateAsync(Customer);
                    addEditWindow.Close();
                    Customer = new Customer();
                    MessageBox.Show("L’ajout de clients a été couronné de succès");

                }
                else
                {
                    await _customerRepository.UpdateAsync(Customer);
                    addEditWindow.Close();
                    Customer = new Customer();
                    MessageBox.Show("L’modifié de clients a été couronné de succès");
                }
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
                if (SelectedCustomer is null)
                {
                    MessageBox.Show("S’il vous plaît sélectionner un client");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($" Voulez-vous supprimer le client {SelectedCustomer.FullName} ? ", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _customerRepository.DeleteAsync(SelectedCustomer.Id);
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
