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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestDatabaseConnection();

        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the RegisterWindom page

            // Navigate to the RegisterWindom page within the MainFrame
            MainFrame.Navigate(new Uri("RegisterWindom.xaml", UriKind.Relative));
        }






        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the LoginPage
            MainFrame.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
        }



        private void TestDatabaseConnection()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Attempt to open the connection
                    MessageBox.Show("Database connection successful.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
