using System;
using System.Windows;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; // Change to use MySQL
using Fitfinder;
using System.Windows.Controls;
using MySqlX.XDevAPI;
using System.Xml.Linq;

namespace Fitfinder
{
    public partial class RegisterWindom : Page
    {
        private string name;
        private string surname;
        private string email;
        private string password;
        private Data _database; // You can keep your Data class instance if needed

        public RegisterWindom()
        {
            InitializeComponent();
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;
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
                MessageBox.Show("Trainee registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private void TrainerRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get additional trainer registration information from the second page
            string location = txtTrainerLocation.Text;
            int genderId = 0; // Initialize with 0 which means no gender selected
            if (rbFemale.IsChecked == true)
                genderId = 1; // Female
            else if (rbMale.IsChecked == true)
                genderId = 2; // Male
            else if (rbOther.IsChecked == true)
                genderId = 3; // Other
            else
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            int price;
            if (!int.TryParse(txtTraineePrice.Text, out price))
            {
                MessageBox.Show("Price must be a valid integer.");
                return;
            }
            string workouts = "";
            foreach (ListBoxItem item in lstWorkouts.SelectedItems)
            {
                workouts += item.Content.ToString() + ", ";
            }
            if (workouts.Length > 0)
            {
                workouts = workouts.Substring(0, workouts.Length - 2); // Remove trailing comma and space
            }
            else
            {
                MessageBox.Show("Please select at least one workout type.");
                return;
            }

            // Create a Trainer object using all information
            Trainer trainer = new Trainer(0, name, surname, email, password, null, genderId, "Trainer description", location, price, 5, "Trainer certifications", true);

            try
            {
                MessageBox.Show("Trainer registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }
    }
}
