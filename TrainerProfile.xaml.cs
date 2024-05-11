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

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for TrainerProfile.xaml
    /// </summary>
    public partial class TrainerProfile : Page
    {
        private YourProfil profil = new YourProfil();

        public TrainerProfile()
        {
            InitializeComponent();
            LoadProfilePicture();
            LoadUserProfile();
        }

        private void Back_button(object sender, RoutedEventArgs e)
        {
            TrainerMainPage trainerMainPage = new TrainerMainPage();
            this.NavigationService.Navigate(trainerMainPage);
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
    }
}
