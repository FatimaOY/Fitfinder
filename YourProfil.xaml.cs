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
        private void LoadUserProfile()
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

        private void LoadProfilePicture()
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

    }
}
