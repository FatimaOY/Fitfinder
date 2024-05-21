using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics.Eventing.Reader;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for TrainerProfile.xaml
    /// </summary>
    public partial class TrainerProfile : Page
    {
        private ObservableCollection<string> workoutTypes = new ObservableCollection<string>();
        private int trainerId;
        private YourProfil profil = new YourProfil();

        public TrainerProfile()
        {
            
            InitializeComponent();
            LoadTrainerProfile();
            LoadProfilePicture();
            

        }
        public void LoadTrainerProfile()
        {
            var currentUser = UserSession.CurrentUser;

            if (currentUser != null)
            {
                if (UserSession.UserRole == "Trainer")
                {
                    var currentTrainer = TrainerSession.CurrentTrainer;
                    NameTextBlock.Text = $"Name: {currentUser.Name}";
                    SurnameTextBlock.Text = $"Surname: {currentUser.Surname}";
                    EmailTextBlock.Text = $"Email: {currentUser.Email}";
                    LocationTextBlock.Text = $"Location: {currentTrainer.Location}";
                    PriceTextBlock.Text = $"Price: {currentTrainer.Price}";
                    MessageBox.Show($"User role: {UserSession.UserRole}", "User Role", MessageBoxButton.OK, MessageBoxImage.Information);
                    DescriptionTextBlock.Text = $"Description: {currentTrainer.Description}";
                    string savedEmail = currentUser.Email;

                    // Instantiate TrainerInfo object
                    TrainerInfo trainerInfo = new TrainerInfo();
                    
                    trainerInfo.TrainerId = currentTrainer.TrainerId;
                    trainerId = currentTrainer.TrainerId;
                    
                    MessageBox.Show(Convert.ToString(currentTrainer.TrainerId));

                 

                    // Update trainer styles
                    List <string> stylesList = UpdateTrainerStyles(trainerId);
                    
                    

                    // Set the item source for the WorkoutTypesListBox
                    foreach (var style in TrainerSession.styles)
                    {
                        WorkoutTypesListBox.Items.Add(style);
                    }
                }
            }
            else
            {
                MessageBox.Show("User information could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public string GetWorkoutTypeNameById(int workoutTypeId)
        {
            string workoutTypeName = null;

            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;"; // Replace this with your actual connection string
            string query = "SELECT Name FROM Workouttypes WHERE WorkoutTypeId = @WorkoutTypeId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutTypeId", workoutTypeId);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        workoutTypeName = Convert.ToString(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while getting workout type name: " + ex.Message);
                }
            }

            return workoutTypeName;
        }
        public List<int> GetWorkoutIdsForTrainer(int trainerId)
        {
            List<int> workoutIds = new List<int>();

            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;"; // Replace this with your actual connection string
            string query = "SELECT WorkoutType FROM trainerworkout WHERE PersonalTrainer = @PersonalTrainer";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonalTrainer", trainerId);

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int workoutId = reader.GetInt32("WorkoutType");
                            workoutIds.Add(workoutId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while retrieving workout IDs: " + ex.Message);
                }
            }

            return workoutIds;
        }
        public List<string> UpdateTrainerStyles(int trainerId)
        {
            List<int> workoutIds = GetWorkoutIdsForTrainer(trainerId);
            List<string> stylesList = new List<string>();

            // Iterate over the workout IDs and get the corresponding names
            foreach (int workoutId in workoutIds)
            {
                string workoutName = GetWorkoutTypeNameById(workoutId);
                stylesList.Add(workoutName);
                MessageBox.Show(workoutName);

            }

            // Update the styles list in the TrainerInfo object
            TrainerSession.styles = stylesList;
            return stylesList;

        }

        private void Back_button(object sender, RoutedEventArgs e)
        {
            TrainerMainPage trainerMainPage = new TrainerMainPage(trainerId);
            this.NavigationService.Navigate(trainerMainPage);
        }
        private void ChangeDescription_Click(object sender, RoutedEventArgs e)
        {

            Window updateDescriptionWindow = new Window();
            updateDescriptionWindow.Title = "Update Description";
            updateDescriptionWindow.Width = 300;
            updateDescriptionWindow.Height = 200;

            // Create a StackPanel to hold UI elements
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10);

            // Add a TextBox for user input
            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Margin = new Thickness(0, 0, 0, 10);
            descriptionTextBox.Width = 200;
            descriptionTextBox.Height = 100;
            descriptionTextBox.TextWrapping = TextWrapping.Wrap;
            descriptionTextBox.AcceptsReturn = true;
            stackPanel.Children.Add(descriptionTextBox);

            // Add a Button to confirm the description update
            Button confirmButton = new Button();
            confirmButton.Content = "Update";
            confirmButton.Click += (confirmSender, confirmArgs) =>
            {
                // Get the new description from the TextBox
                string newDescription = descriptionTextBox.Text;

                // Call the method to update the description
                var currentUser = UserSession.CurrentUser;
                int userId = currentUser.userId;

                UpdateTrainerDescription(userId, newDescription);

                // Close the pop-up dialog box
                updateDescriptionWindow.Close();
            };
            stackPanel.Children.Add(confirmButton);

            // Add the StackPanel to the Window
            updateDescriptionWindow.Content = stackPanel;

            // Show the pop-up dialog box
            updateDescriptionWindow.ShowDialog();
        }

        private void UploadProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter for file extension and default file extension
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = openFileDialog.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = openFileDialog.FileName;

                // Display selected image in the Image control
                ProfilePicture.Source = new BitmapImage(new Uri(filename));

                // Convert the selected image file to byte array
                byte[] imageData = File.ReadAllBytes(filename);

                // Insert the byte array (image data) into the database
                profil.InsertProfilePictureToDatabase(imageData);
            }
        }

        public void LoadUserProfile()
        {
            var currentUser = UserSession.CurrentUser;

            if (currentUser != null)
            {
                NameTextBlock.Text = $"Name: {currentUser.Name}";
                SurnameTextBlock.Text = $"Surname: {currentUser.Surname}";
                EmailTextBlock.Text = $"Email: {currentUser.Email}";
                string savedEmail = currentUser.Email;
                // If you have a password field, it's usually a placeholder for user interaction (e.g., change password)
                // Avoid displaying plain-text passwords
            }
            else
            {
                MessageBox.Show("User information could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void UpdateTrainerDescription(int userId, string newDescription)
        {
            string connectionString = "datasource=127.0.0.1;" +
                                      "port=3306;" +
                                      "username=root;" +
                                      "password=;" +
                                      "database=fitfinder4";

            string query = "UPDATE Trainer SET Description = @newDescription WHERE PersonId = @userId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@newDescription", newDescription);
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Description updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No rows updated. User not found or no changes made to description.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
       
        public void LoadProfilePicture()
        {
            var currentUser = UserSession.CurrentUser;

            if (currentUser != null)
            {
                // Connection string to your database
                string connectionString =
                    "datasource=127.0.0.1;" +
                    "port=3306;" +
                    "username=root;" +
                    "password=;" +
                    "database=fitfinder4";

                string query = "SELECT ProfilePic FROM user WHERE UserID = @UserID";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        Data data = new Data(); // Create an instance of the Data class

                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@UserID", data.GetUserId((currentUser.Email).ToString(), (currentUser.Password).ToString()));

                        connection.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            byte[] imageData = (byte[])result;
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(imageData);
                            bitmapImage.EndInit();

                            ProfilePicture.Source = bitmapImage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading profile picture: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void change_password_button(object sender, RoutedEventArgs e)
        {
            changePasswordPage changePassword = new changePasswordPage();
            this.NavigationService.Navigate(changePassword);
        }

        private void delete_profile_button(object sender, RoutedEventArgs e)
        {
            var currentUser = UserSession.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("No user is currently logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete your profile?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string connectionString =
                    "datasource=127.0.0.1;" +
                    "port=3306;" +
                    "username=root;" +
                    "password=;" +
                    "database=fitfinder4";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string deleteUserQuery = "DELETE FROM user WHERE UserID = @UserID";
                        MySqlCommand deleteUserCmd = new MySqlCommand(deleteUserQuery, connection);
                        deleteUserCmd.Parameters.AddWithValue("@UserID", currentUser.userId);

                        int rowsAffected = deleteUserCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Optionally, navigate to a different page after deletion, e.g., the login page
                            // Navigate to MainWindow
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();

                            // Close the current window
                            Window.GetWindow(this).Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete profile. No rows affected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting profile: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
