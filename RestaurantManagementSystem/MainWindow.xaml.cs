using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.ViewModels;
using System.Windows;

namespace RestaurantManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerViewModel customerViewModel;
        private readonly DishViewModel dishViewModel;
        private readonly OrderViewModel orderViewModel;
        private readonly DashboardViewModel dashboardViewModel;
        public MainWindow()
        {
            InitializeComponent();
            var db = new RestaurantManagementSystemDbContext();
            db.Database.Migrate();
            customerViewModel = new CustomerViewModel();
            dishViewModel = new DishViewModel();
            dashboardViewModel = new();
            orderViewModel = new();
            CustomerBtn.DataContext = CustomerCanvas.DataContext = customerViewModel;
            DishCanvas.DataContext = DishBtn.DataContext = dishViewModel;
            OrderBtn.DataContext = OrderCanvas.DataContext = orderViewModel;
            DashboardBtn.DataContext = DashboardCanvas.DataContext = dashboardViewModel;
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
            OrderCanvas.Visibility = Visibility.Hidden;
            DashboardCanvas.Visibility = Visibility.Hidden;


        }

        private void DishBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerCanvas.Visibility = Visibility.Hidden;
            DishCanvas.Visibility = Visibility.Visible;
            OrderCanvas.Visibility = Visibility.Hidden;
            DashboardCanvas.Visibility = Visibility.Hidden;


        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerCanvas.Visibility = Visibility.Hidden;
            DishCanvas.Visibility = Visibility.Hidden;
            OrderCanvas.Visibility = Visibility.Visible;
            DashboardCanvas.Visibility = Visibility.Hidden;

        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerCanvas.Visibility = Visibility.Hidden;
            DishCanvas.Visibility = Visibility.Hidden;
            OrderCanvas.Visibility = Visibility.Hidden;
            DashboardCanvas.Visibility = Visibility.Visible;

        }
    }
}