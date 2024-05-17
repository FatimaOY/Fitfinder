using System;
using System.Windows;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; // Change to use MySQL
using Fitfinder;
using System.Windows.Controls;
using MySqlX.XDevAPI;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Fitfinder
{
    public partial class RegisterWindom : Page
    {
        private List<WorkoutType> workoutTypes = new List<WorkoutType>
        {
            new WorkoutType(1, "Weightlifting"),
            new WorkoutType(2, "Cardio"),
            new WorkoutType(3, "Stretching"),
            new WorkoutType(4, "Yoga"),
            new WorkoutType(5, "Pilates"),
            new WorkoutType(6, "CrossFit"),
            new WorkoutType(7, "Calinistics"),
            new WorkoutType(8, "Swimming")
        };
        private MyViewModel _viewModel; // Declare the ViewModel
        private Data _database; // You can keep your Data class instance if needed

        public RegisterWindom()
        {
            InitializeComponent();
            _viewModel = new MyViewModel(); // Initialize the ViewModel
            _database = new Data(); // Initialize the Data class if needed
            //lstWorkouts.ItemsSource = workoutTypes;
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
            if (IfEmailExists(email) == true)
            {
                MessageBox.Show("An account with this email already exists.");
                return;
            }
            int genderId = GetGenderIdClient();

            // Create a Client object
            Client client = new Client(0, name, surname, email, password, null, genderId, null, null);

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewClient(client); // Adding a client
                MessageBox.Show("Trainee registered successfully.");

                // Navigate to the login page again
                LoginPage loginPage = new LoginPage();
                this.NavigationService.Navigate(loginPage);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }
        private byte[] imageData; // Declare imageData at the class level
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

        private void Trainer_final_register_click(object sender, RoutedEventArgs e)
        {
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string email = txtTrainerEmail.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;
            /*string location = txtTrainerLocation.Text; // ADD HERE GENDER ID WHEN READY
            int price = Convert.ToInt32(txtTraineePrice.Text);*/

            // Validate input (check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            if (IfEmailExists(email) == true)
            {
                MessageBox.Show("An account with this email already exists.");
                return;
            }

            int genderId = GetGenderIdTrainer();

            // Ensure imageData is not null
            if (imageData == null)
            {
                MessageBox.Show("Please upload a profile picture.");
                return;
            }

            // Create a Trainer object
            Trainer trainer = new Trainer(0, name, surname, email, password, imageData, genderId, null, null, 0);
            

            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewTrainer(trainer); // Adding a trainer
                MessageBox.Show("Trainer registered successfully.");


                Questions1 questionsPage = new Questions1(email,password);
                this.NavigationService.Navigate(questionsPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        public bool IfEmailExists(string email)
        {
            // Assuming you have a connection string defined somewhere
            string connectionString = "server=127.0.0.1;port=3306;database=fitfinder4;user=root;password=;";

            // SQL query to check if the email exists
            string query = "SELECT COUNT(*) FROM User WHERE Email = @Email";

            // Variable to store the count
            int count = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Create a MySqlCommand object
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add the parameter for email
                    command.Parameters.AddWithValue("@Email", email);

                    // Open the connection
                    connection.Open();

                    // Execute the query and store the result in count
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            // If the count is greater than 0, email exists; otherwise, it doesn't
            return count > 0;
        }
        private void UploadProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter for file extension and default file extension
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = openFileDialog.ShowDialog();

            // Get the selected file name and display in an Image control
            if (result == true)
            {
                // Open document
                string filename = openFileDialog.FileName;

                // Display selected image in the Image control
                ProfilePicture.Source = new BitmapImage(new Uri(filename));

                // Convert the selected image file to byte array
                imageData = File.ReadAllBytes(filename);
            }
        }
    }
}
