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

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for AdminCreateTrainee.xaml
    /// </summary>
    public partial class AdminCreateTrainee : Page
    {
        Data data = new Data(); // Create an instance of the Data class
        RegisterWindom registerWindow = new RegisterWindom(); // Create an instance of the Data class
        private MyViewModel _viewModel; // Declare the ViewModel

        public AdminCreateTrainee()
        {
            InitializeComponent();
            _viewModel = new MyViewModel(); // Initialize the ViewModel

        }

        private void CreateTrainee_button(object sender, RoutedEventArgs e)
        {
            // Get trainee registration information
            string name = txtTraineeName.Text;
            string surname = txtTraineeSurname.Text;
            string email = txtTraineeEmail.Text;
            string password = txtTraineePassword.Password;
            string confirmPassword = txtTraineeConfirmPassword.Password;

            // Validate input (check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            if (registerWindow.IfEmailExists(email) == true)
            {
                MessageBox.Show("An account with this email already exists.");
                return;
            }
            int genderId = GetGenderIdClient();

            // Create a Client object
            Client client = new Client(0, name, surname, email, password, null, genderId, "Trainee description", null);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewClient(client); // Adding a client
                MessageBox.Show("Trainee Created successfully!");

                // Navigate to the login page again
                AdminMainPage adminMainPage = new AdminMainPage();
                this.NavigationService.Navigate(adminMainPage);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private int GetGenderIdClient()
        {
            if (Female.IsChecked == true) return 1; // Female
            if (Male.IsChecked == true) return 2; // Male
            if (Other.IsChecked == true) return 3; // Other
            return 0; // Default or unknown
        }
    }
}
