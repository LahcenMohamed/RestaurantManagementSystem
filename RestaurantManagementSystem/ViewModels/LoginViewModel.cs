using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using RestaurantManagementSystem.Models;

using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.ViewModels
{
    public sealed class LoginViewModel
    {
        private readonly AccountRepository accountRepository;
        private readonly LoginWindow loginWindow;

        public User User { get; set; }

        public ICommand Check { get; }

        public LoginViewModel(LoginWindow login)
        {
            accountRepository = new AccountRepository();
            loginWindow = login;
            Check = new RelayCommand(check);
            User = new User();
        }

        private async void check()
        {
            if (await accountRepository.Login(User))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                loginWindow.Close();
            }
            else
            {
                MessageBox.Show("UserName or Password is not corract");
            }
        }
    }
}
