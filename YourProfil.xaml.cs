using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
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
using System.IO;


namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for YourProfil.xaml
    /// </summary>
    public partial class YourProfil : Page
    {
        public YourProfil()
        {
            InitializeComponent();
            LoadUserProfile();
            LoadProfilePicture();
        }
        public void LoadUserProfile()
        {
            var currentUser = UserSession.CurrentUser;

            if (currentUser != null)
            {
                if (UserSession.UserRole == "Client")
                {
                    var currentClient = ClientSession.CurrentClient;
                    NameTextBlock.Text = $"Name: {currentUser.Name}";
                    SurnameTextBlock.Text = $"Surname: {currentUser.Surname}";
                    EmailTextBlock.Text = $"Email: {currentUser.Email}";
                    //MessageBox.Show($"User role: {UserSession.UserRole}", "User Role", MessageBoxButton.OK, MessageBoxImage.Information);
                    DescriptionTextBlock.Text = $"Description: {currentClient.Description}";
                    string savedEmail = currentUser.Email;
                    // If you have a password field, it's usually a placeholder for user interaction (e.g., change password)
                    // Avoid displaying plain-text passwords
                }
            }
            else
            {
                MessageBox.Show("User information could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Profile_button(object sender, RoutedEventArgs e)
        {
            YourProfil yourProfil = new YourProfil();
            this.NavigationService.Navigate(yourProfil);
        }

        private void BrowseTrainer_button(object sender, RoutedEventArgs e)
        {
            BrowseTrainers browseTrainers = new BrowseTrainers();
            this.NavigationService.Navigate(browseTrainers);
        }


        private void YourWorkouts_button(object sender, RoutedEventArgs e)
        {
            YourWorkouts yourWorkouts = new YourWorkouts();
            this.NavigationService.Navigate(yourWorkouts);
        }

        private void Messages_button(object sender, RoutedEventArgs e)
        {
            MessagesTrainee messagesTrainee = new MessagesTrainee();
            this.NavigationService.Navigate(messagesTrainee);
        }

        private void change_password_button(object sender, RoutedEventArgs e)
        {
            changePasswordPage changepasswordPage = new changePasswordPage();
            this.NavigationService.Navigate(changepasswordPage);
        }
        private void UpdateDescription_Click(object sender, RoutedEventArgs e)
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

                bool success = UpdateClientDescription(userId, newDescription);

                if (success)
                {
                    // Close the pop-up dialog box
                    /*updateDescriptionWindow.Close();

                    // Log out the user (clear the session)
                    UserSession.CurrentUser = null;

                    // Navigate to the login window
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.ContentFrame.Navigate(new Uri("MainWindow.xaml", UriKind.Relative));
                    }*/
                    // Instantiate the MainWindow
                    MainWindow mainWindow = new MainWindow();

                    // Close the current window (assuming the updateDescriptionWindow is the current window)
                    updateDescriptionWindow.Close();

                    // Set the MainWindow as the application's main window
                    Application.Current.MainWindow = mainWindow;

                    // Get the parent window
                    Window parentWindow = Window.GetWindow(this);

                    // Close the parent window if it's not null
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }

                    // Show the MainWindow
                    mainWindow.Show();

                }
                else
                {
                    // Display a message indicating that the description update failed
                    MessageBox.Show("Failed to update description. Please try again.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                };
                stackPanel.Children.Add(confirmButton);

                // Add the StackPanel to the Window
                updateDescriptionWindow.Content = stackPanel;

                // Show the pop-up dialog box
                updateDescriptionWindow.ShowDialog();
            }
            

        bool UpdateClientDescription(int userId, string newDescription)
        {
            string connectionString = "datasource=127.0.0.1;" +
                                      "port=3306;" +
                                      "username=root;" +
                                      "password=;" +
                                      "database=fitfinder4";

            string query = "UPDATE Client SET Description = @newDescription WHERE PersonId = @userId";

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
                        MessageBox.Show("Description updated successfully. You will get logged out when clicking the button \'OK\'. Log in again to  ");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("No rows updated. User not found or no changes made to description.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    return false;
                }
            }
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
                InsertProfilePictureToDatabase(imageData);
            }
        }

        public void InsertProfilePictureToDatabase(byte[] imageData)
        {
            try
            {
                var currentUser = UserSession.CurrentUser;
                // Connection string to your database
                string connectionString =
                    "datasource=127.0.0.1;" +
                    "port=3306;" +
                    "username=root;" +
                    "password=;" +
                    "database=fitfinder4";

                // SQL query to update user profile picture
                string query = "UPDATE user SET ProfilePic = @ProfilePic WHERE UserID = @UserID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    Data data = new Data(); // Create an instance of the Data class
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ProfilePic", imageData);
                    cmd.Parameters.AddWithValue("@UserID", data.GetUserId((currentUser.Email).ToString(),(currentUser.Password).ToString())); // Ensure you provide the UserID

                    connection.Open();

                    // Execute the command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if the update was successful
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile picture inserted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert profile picture. No rows affected.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
