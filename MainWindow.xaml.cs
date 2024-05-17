using System;
using System.Collections.Generic;
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
using MySql.Data.MySqlClient;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";

        public MainWindow()
        {
            InitializeComponent();
            TestDatabaseConnection();

        }
        public Frame ContentFrame
        {
            get { return this.MainContentFrame; }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the RegisterWindom page

            // Navigate to the RegisterWindom page within the MainFrame
            MainFrame.Navigate(new Uri("RegisterWindom.xaml", UriKind.Relative));
        }


        public bool ValidateUserLogin(string email, string password)
        {
            // Logic to validate user login
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; // True if user is found
                }
            }
        }
        public UserInfo GetUserByEmail(string email)
        {
            // Fetch user information by email
            string query = "SELECT * FROM Users WHERE Email = @Email";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserInfo
                            {
                                userId = (int)reader["UserID"],
                                Name = reader["Name"]?.ToString(),
                                Surname = reader["Surname"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                            };
                        }
                    }
                }
            }
            return null; // Return null if no user is found
        }
        public string GetUserRole(int userId)
        {
            // Check if user is a client, trainer, or admin
            if (CheckUserInTable("Clients", userId))
            {
                return "Client";
            }
            if (CheckUserInTable("Trainers", userId))
            {
                return "Trainer";
            }
            if (CheckUserInTable("Admins", userId))
            {
                return "Admin";
            }

            return "Unknown"; // Default role if not found in any table
        }

        private bool CheckUserInTable(string tableName, int userId)
        {
            // Check if a user ID is present in a specific table
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE UserID = @UserID";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; // True if the user is found in the specified table
                }
            }
        }




        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the LoginPage
            MainFrame.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
        }



        private void TestDatabaseConnection()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Attempt to open the connection
                    MessageBox.Show("Database connection successful.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void UpdatePassword(int userId, string newPassword, string currentPassword)
        {
            using (var connection = new MySqlConnection(connectionString)) // Create a new connection
            {
                connection.Open(); // Open the connection

                string query = "SELECT Password FROM User WHERE UserID = @UserID";
                using (MySqlCommand command = new MySqlCommand(query, connection)) // Use the new connection
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    string storedPassword = command.ExecuteScalar()?.ToString();
                    

                    if (storedPassword != currentPassword)
                    {
                        MessageBox.Show("Current password is incorrect.");
                        return;
                    }

                    // If passwords match, update to the new password
                    query = "UPDATE User SET Password = @NewPassword WHERE UserID = @UserID";
                    using (MySqlCommand updateCommand = new MySqlCommand(query, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                        updateCommand.Parameters.AddWithValue("@UserID", userId);
                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Password updated successfully.");
                    }
                }
            }
        }

    }
}
