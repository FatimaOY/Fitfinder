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

        // Method to determine the gender based on the selected radio button
        private int GetGenderIdTrainer()
        {
            if (rbFemale.IsChecked == true) return 1; // Female
            if (rbMale.IsChecked == true) return 2; // Male
            if (rbOther.IsChecked == true) return 3; // Other
            return 0; // Default or unknown
        }
        private int GetGenderIdClient()
        {
            if (Female.IsChecked == true) return 1; // Female
            if (Male.IsChecked == true) return 2; // Male
            if (Other.IsChecked == true) return 3; // Other
            return 0; // Default or unknown
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
            int genderId = GetGenderIdClient();

            // Create a Client object
            Client client = new Client(0, name, surname, email, password, null, genderId, "Trainee description", null);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewClient(client); // Adding a client
                MessageBox.Show("Trainee registered successfully.");

                //The navigation to the browseTrainers page
                BrowseTrainers browseTrainers = new BrowseTrainers();
                this.NavigationService.Navigate(browseTrainers);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private void Trainer_final_register_click(object sender, RoutedEventArgs e)
        {
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string email = txtTrainerEmail.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;
            string location = txtTrainerLocation.Text; //ADD HERE GEDER ID WHEN READY
            int price = Convert.ToInt32(txtTraineePrice.Text);

            
            // Validate input (check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            int genderId = GetGenderIdTrainer();

            // Create a Trainer object
            Trainer trainer = new Trainer(0,name, surname, email, password, null, genderId, "Trainer description", location, price);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewTrainer(trainer); // Adding a trainer
                MessageBox.Show("Trainer registered successfully.");

                //The navigation to the browseTrainers page
                TrainerMainPage trainerMainPage = new TrainerMainPage();
                this.NavigationService.Navigate(trainerMainPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

    }
}
