using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories.IReposioriesHelpers;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.IdentityModel.Tokens;
using System.Windows.Media;
using Microsoft.Win32;
using System.Collections.Specialized;

namespace RestaurantManagementSystem.ViewModels
{
    public sealed class DishViewModel : INotifyCollectionChanged
    {
        private readonly IDishRepository _dishRepository;
        private string searchItem;
        private AddEditDish addEditDish;
        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                onPropertyChanged(nameof(ImageSource));
            }
        }
        public Dish Dish { get; set; }
        public Dish SelectedDish { get; set; }
        public ObservableCollection<Dish> Dishs { get; set; }
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

        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand GetAllCommand { get; }
        public ICommand SelectImageCommand { get; }
        public DishViewModel()
        {
            _dishRepository = new DishRepository();
            Dish = new Dish();
            Dishs = new ObservableCollection<Dish>();
            searchItem = string.Empty;
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SaveCommand = new RelayCommand(Save);
            GetAllCommand = new RelayCommand(GetAll);
            SelectImageCommand = new RelayCommand(SelectImage);
        }

        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                Dish.ImageUrl = ImageSource = openFileDialog.FileName ;
            }
        }

        private async void SearchAsync()
        {
            Dishs.Clear();
            await foreach (var customer in _dishRepository.Search(searchItem))
            {
                Dishs.Add(customer);
            }
        }
        private async void GetAll()
        {
            if (!Dishs.IsNullOrEmpty())
                return;
            await foreach (var item in _dishRepository.GetAllAsync())
            {
                Dishs.Add(item);
            }
        }
        private async Task GetAllForOp()
        {
            Dishs.Clear();
            await foreach (var item in _dishRepository.GetAllAsync())
            {
                Dishs.Add(item);
            }
        }
        private void Add()
        {
            SelectedDish = null;
            addEditDish = new AddEditDish(this);
            addEditDish.Show();
        }

        public void Update()
        {
            if (SelectedDish is null)
            {
                MessageBox.Show("S’il vous plaît sélectionner un client");
                return;
            }
            Dish = SelectedDish;
            addEditDish = new AddEditDish(this);
            addEditDish.Show();
        }

        public async void Save()
        {
            try
            {
                if (SelectedDish is null)
                {
                    await _dishRepository.CreateAsync(Dish);
                    Dish = new Dish();
                    MessageBox.Show("L’ajout de plat a été couronné de succès");

                }
                else
                {
                    await _dishRepository.UpdateAsync(Dish);
                    Dish = new Dish();
                    MessageBox.Show("L’modifié de plat a été couronné de succès");
                }
                addEditDish.Close();
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
                if (SelectedDish is null)
                {
                    MessageBox.Show("S’il vous plaît sélectionner un plat");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($" Voulez-vous supprimer le plat {SelectedDish.Name} ? ", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _dishRepository.DeleteAsync(SelectedDish.Id);
                    await GetAllForOp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur");
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
