using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.IO;



namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for AdminEditProfile.xaml
    /// </summary>
    public partial class AdminEditProfile : Page
    {
        private AdminMainPage.User userToUpdate;
        private byte[] selectedProfilePic; // Store selected profile picture as byte array


        public AdminEditProfile(AdminMainPage.User selectedUser)
        {
            InitializeComponent();

            // Initialize the userToUpdate field with the selectedUser object passed from AdminMainPage
            userToUpdate = selectedUser;

            // Display the user information
            DisplayUserInfo();
        }

        private void DisplayUserInfo()
        {
            userToUpdate = AdminMainPage.SelectedUser;

            if (userToUpdate != null)
            {
                txtName.Text = userToUpdate.Name;
                txtSurname.Text = userToUpdate.Surname;
                txtEmail.Text = userToUpdate.Email;
                txtDescription.Text = userToUpdate.Description;
                txtPassword.Text = userToUpdate.Password;
                // Set selected item based on user's gender
                foreach (ComboBoxItem item in cmbGender.Items)
                {
                    if (Convert.ToInt32(item.Tag) == userToUpdate.GenderId)
                    {
                        cmbGender.SelectedItem = item;
                        break;
                    }
                }
                // Set other fields as needed
            }

            // Set profile picture
            if (userToUpdate.ProfilePic != null && userToUpdate.ProfilePic.Length > 0)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(userToUpdate.ProfilePic);
                bitmap.EndInit();
                imgProfile.Source = bitmap;
            }
        }

        public class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }

            public string Password { get; set; }
            public byte[] ProfilePic { get; set; }

            public int GenderId { get; set; }

            public string Gender { get; set; } // This will store the gender display value

            public string Description { get; set; }


            // Add more properties as needed
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user information from UI elements
            string name = txtName.Text;
            string surname = txtSurname.Text;
            string email = txtEmail.Text;
            string description = txtDescription.Text;
            string password = txtPassword.Text;
            int genderId = 0; // Initialize genderId variable


            // Update profile picture only if selected
            byte[] profilePic = selectedProfilePic ?? userToUpdate.ProfilePic;

            // Determine genderId based on selected item in ComboBox
            ComboBoxItem selectedGenderItem = cmbGender.SelectedItem as ComboBoxItem;
            if (selectedGenderItem != null)
            {
                // Retrieve the Tag property of the selected item
                string genderTag = selectedGenderItem.Tag?.ToString();
                if (!string.IsNullOrEmpty(genderTag))
                {
                    // Parse the Tag value to an integer and assign it to genderId
                    genderId = int.Parse(genderTag);
                }
            }

            // Create a new User object with the updated information
            User updatedUser = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Description = description,
                ProfilePic = profilePic,
                Password = password,
                GenderId = genderId,


                // Set other fields as needed
            };

            // Pass the updatedUser object to the UpdateUserInDatabase method
            UpdateUserInDatabase(updatedUser);

            //navigates again to the list of trainees
            AdminMainPage adminMainPageListForTrainee = new AdminMainPage();
            this.NavigationService.Navigate(adminMainPageListForTrainee);
        }


        public void UpdateUserInDatabase(User user)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE user SET name = @Name, surname = @Surname, email = @Email, Password = @Password, GenderId=@GenderID, ProfilePic = @ProfilePic WHERE email = @Email; UPDATE client SET Description = @Description WHERE PersonId = (SELECT UserID FROM user WHERE email = @Email);";

                


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Surname", user.Surname);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Description", user.Description);
                    command.Parameters.AddWithValue("@ProfilePic", user.ProfilePic);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    command.Parameters.AddWithValue("@GenderId", user.GenderId);



                    // Add other parameters as needed

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user.");
                    }
                }
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                selectedProfilePic = File.ReadAllBytes(fileName);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fileName);
                bitmap.EndInit();
                imgProfile.Source = bitmap;
            }
        }
    }
}
