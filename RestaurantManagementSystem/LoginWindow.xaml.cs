using RestaurantManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool _ispass = true;
        private readonly LoginViewModel loginViewModel;

        public LoginWindow()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel(this);
            DataContext = loginViewModel;
        }
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_ispass)
            {
                TextPassordBox.Text = passwordBox.Password;
                loginViewModel.User.Password = TextPassordBox.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_ispass)
            {
                TextPassordBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordBox.Visibility = Visibility.Visible;
                TextPassordBox.Visibility = Visibility.Hidden;
            }
            _ispass = !_ispass;
        }

        private void TextPassordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_ispass)
            {
                passwordBox.Password = TextPassordBox.Text;
                loginViewModel.User.Password = TextPassordBox.Text;
            }
        }
    }
}
