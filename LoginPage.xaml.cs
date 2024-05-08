using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient; // MySQL connection
using System.Security.Cryptography; // For hashing
using System.Text; // For converting byte arrays to strings

namespace Fitfinder
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }

    public static class UserSession
    {
        public static UserInfo CurrentUser { get; set; }
        public static string UserRole { get; set; }
    }

    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    /// 
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Password reset instructions will be sent to your email.", "Forgot Password");
        }



        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailInput.Text;
            string password = PasswordInput.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Visibility = Visibility.Visible;
                return;
            }

           
            string connectionString =
                                "datasource=127.0.0.1;" +
                                "port=3306;" +
                                "username=root;" +
                                "password=;" +
                                "database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM User WHERE email = @Email AND password = @Password";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine(cmd.CommandText);

                    if (reader.Read())
                        {
                        UserSession.CurrentUser = new UserInfo
                        {
                            Email = reader["email"].ToString(),
                            FirstName = reader["name"].ToString(),
                            Surname = reader["surname"].ToString(),
                        };
                        // Successful login
                        MessageBox.Show("Login successful!", "Login");
                        // Navigate to another page or perform some action
                        BrowseTrainers browseTrainers = new BrowseTrainers();
                        this.NavigationService.Navigate(browseTrainers);

                    }
                    else
                    {
                        lblError.Visibility = Visibility.Visible; // Show error message
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
