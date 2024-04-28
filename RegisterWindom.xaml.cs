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
        // Connection string to your MySQL database
        private Data database = new Data(); // Assuming Data class has the InsertUser method

        public RegisterWindom()
        {
            InitializeComponent();
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

            // Validate input (for example, check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Create a User object for a trainee
            User trainee = new Client(1,name, surname, email, password, null, 1, null, null); // GenderId as example


            // Insert into the database
            try
            {
                database.InsertUser(trainee); // Use InsertUser to insert the trainee
                MessageBox.Show("Trainee registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private void TrainerNextButton_Click(object sender, RoutedEventArgs e)
        {
            // Get trainer registration information
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string email = txtTrainerEmail.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;

            // Validate input (for example, check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Create a User object for a trainer
            User trainer = new User(name, surname,email,password, null, 1); // Example GenderId

            // Insert into the database
            try
            {
                database.InsertUser(trainer); // Insert the trainer into the database
                MessageBox.Show("Trainer registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

    }
}
