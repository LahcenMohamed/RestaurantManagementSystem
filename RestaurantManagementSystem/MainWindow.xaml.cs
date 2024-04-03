using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerViewModel customerViewModel;
        private readonly DishViewModel dishViewModel;
        public MainWindow()
        {           
            InitializeComponent();
            customerViewModel = new CustomerViewModel();
            dishViewModel = new DishViewModel();
            CustomerBtn.DataContext = CustomerCanvas.DataContext = customerViewModel;
            DishCanvas.DataContext = DishBtn.DataContext = dishViewModel;
        }

        private void btu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void btu_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerCanvas.Visibility = Visibility.Visible;
            DishCanvas.Visibility = Visibility.Hidden;
        }

        private void DishBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerCanvas.Visibility = Visibility.Hidden;
            DishCanvas.Visibility = Visibility.Visible;
        }
    }
}