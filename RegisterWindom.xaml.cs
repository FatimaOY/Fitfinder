using System;
using System.Windows;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; // Change to use MySQL
using Fitfinder;
using System.Windows.Controls;
using MySqlX.XDevAPI;

namespace Fitfinder
{
    public partial class RegisterWindom : Page
    {
        private MyViewModel _viewModel; // Declare the ViewModel
        private Data _database; // You can keep your Data class instance if needed

        public RegisterWindom()
        {
            InitializeComponent();
            _viewModel = new MyViewModel(); // Initialize the ViewModel
            _database = new Data(); // Initialize the Data class if needed
        }

        private void RegisterTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            TraineeRegistrationForm.Visibility = Visibility.Visible;
            TrainerRegistrationForm.Visibility = Visibility.Collapsed;
        }

        private void RegisterTrainerButton_Click(object sender, RoutedEventArgs e)
        {
            TrainerRegistrationForm.Visibility = Visibility.Visible;
            TraineeRegistrationForm.Visibility = Visibility.Collapsed;
        }

        private void TraineeRegisterButton_Click(object sender, RoutedEventArgs e)
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

            // Create a Client object
            Client client = new Client(0, name, surname, email, password, null, 1, "Trainee description", null);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewClient(client); // Adding a client
                MessageBox.Show("Trainee registered successfully.");
                BrowseTrainers browseTrainers = new BrowseTrainers();
                this.NavigationService.Navigate(browseTrainers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private void TrainerNextButton_Click(object sender, RoutedEventArgs e)
        {
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string email = txtTrainerEmail.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;

            // Validate input (check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Create a Trainer object
            Trainer trainer = new Trainer(0,name, surname, email, password, null, 1, "Trainer description", "Trainer location", 100);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewTrainer(trainer); // Adding a trainer
                MessageBox.Show("Trainer registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

    }
}
