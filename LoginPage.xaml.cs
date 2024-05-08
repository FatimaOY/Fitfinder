﻿using System;
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

            // Get user ID
            int userId = GetUserId(email, password);

            if (userId != -1)
            {
                if (UserExistsInTable("Admin", userId))
                {
                    // Admin login
                    UserSession.CurrentUser = GetUserInformation(email, password);
                    AdminMainPage adminMainPage = new AdminMainPage();
                    this.NavigationService.Navigate(adminMainPage);
                }
                else if (UserExistsInTable("Trainer", userId))
                {
                    // Trainer login
                    UserSession.CurrentUser = GetUserInformation(email, password);
                    TrainerMainPage trainerMainPage = new TrainerMainPage();
                    this.NavigationService.Navigate(trainerMainPage);
                }
                else if (UserExistsInTable("Client", userId))
                {
                    // Client login
                    UserSession.CurrentUser = GetUserInformation(email, password);
                    BrowseTrainers browseTrainers = new BrowseTrainers();
                    this.NavigationService.Navigate(browseTrainers);
                }
                else
                {
                    lblError.Visibility = Visibility.Visible; // Show error message
                }
            }
            else
            {
                lblError.Visibility = Visibility.Visible; // Show error message
            }
        }

        private UserInfo GetUserInformation(string email, string password)
        {
            UserInfo userInfo = null;
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            string query = "SELECT * FROM User WHERE email = @Email AND password = @Password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userInfo = new UserInfo
                        {
                            Email = reader["email"].ToString(),
                            FirstName = reader["name"].ToString(),
                            Surname = reader["surname"].ToString(),
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return userInfo;
        }

        private int GetUserId(string email, string password)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int userId = -1; // Default value indicating user not found

            string query = "SELECT UserId FROM user WHERE Email = @Email AND Password = @Password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return userId;
        }

        private bool UserExistsInTable(string tableName, int userId)
        {
            string query = $"SELECT PersonId FROM {tableName} WHERE PersonId = @UserId";

            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null; // If result is not null, user exists in the table
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    return false;
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
