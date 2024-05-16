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
        public int userId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
    }

    public static class UserSession
    {
        public static UserInfo CurrentUser { get; set; }
        public static string UserRole { get; set; }
    }


    public class ClientInfo
    {
        public int TraineeId { get; set; }
        public string Description { get; set; }
        public int PersonId { get; set; }
    }
    public static class ClientSession
    {
        public static ClientInfo CurrentClient { get; set; }
    }

    public class TrainerInfo
    {
        public int TrainerId { get; set; }
        public string Description { get; set; }
        public int PersonId { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
    }
    public static class TrainerSession
    {
        public static TrainerInfo CurrentTrainer { get; set; }
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
                lblError.Text = "Email and password are required."; // Corrected from `Content` to `Text`
                return;
            }

            // Get user ID
            var data = new Data();
            int userId = data.GetUserId(email, password);

            if (userId != -1)
            {
                var currentUser = GetUserInformation(email, password);
                if (currentUser != null)
                {
                    UserSession.CurrentUser = currentUser;

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null)
                    {
                        if (UserExistsInTable("Admin", userId))
                        {
                            // Admin login
                            UserSession.UserRole = "Admin";
                            AdminMainPage adminMainPage = new AdminMainPage();
                            mainWindow.MainFrame.Navigate(adminMainPage);
                        }
                        else if (UserExistsInTable("Trainer", userId))
                        {
                            // Trainer login
                            UserSession.UserRole = "Trainer";
                            TrainerSession.CurrentTrainer = GetTrainerInformation(userId);
                            TrainerMainPage trainerMainPage = new TrainerMainPage();
                            mainWindow.MainFrame.Navigate(trainerMainPage);
                        }
                        else if (UserExistsInTable("Client", userId))
                        {
                            // Client login
                            UserSession.UserRole = "Client";
                            ClientSession.CurrentClient = GetClientInformation(userId);
                            BrowseTrainers browseTrainers = new BrowseTrainers();
                            mainWindow.MainFrame.Navigate(browseTrainers);
                        }
                        else
                        {
                            lblError.Visibility = Visibility.Visible;
                            lblError.Text = "User role not found."; // Corrected from `Content` to `Text`
                        }
                    }
                    else
                    {
                        lblError.Visibility = Visibility.Visible;
                        lblError.Text = "Main window is not available."; // Corrected from `Content` to `Text`
                    }
                }
                else
                {
                    lblError.Visibility = Visibility.Visible;
                    lblError.Text = "Failed to retrieve user information."; // Corrected from `Content` to `Text`
                }
            }
            else
            {
                lblError.Visibility = Visibility.Visible;
                lblError.Text = "Invalid email or password."; // Corrected from `Content` to `Text`
            }
        }

        UserInfo GetUserInformation(string email, string password)
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
                            userId = (int)reader["UserID"],
                            Email = reader["Email"].ToString(),
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Password = reader["Password"].ToString(),
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

        ClientInfo GetClientInformation(int userId)
        {
            ClientInfo clientInfo = null;
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            string query = "SELECT * FROM Client WHERE PersonId = @userId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);


                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        clientInfo = new ClientInfo
                        {
                           TraineeId = (int)reader["TraineeId"],
                           PersonId = (int)reader["PersonId"],
                           Description = reader["Description"].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return clientInfo;
        }
        public TrainerInfo GetTrainerInformation(int userId)
        {
            TrainerInfo trainerInfo = null;
            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";

            string query = "SELECT * FROM Trainer WHERE PersonId = @userId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);


                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        trainerInfo = new TrainerInfo
                        {
                            TrainerId = (int)reader["TrainerId"],
                            PersonId = (int)reader["PersonId"],
                            Description = reader["Description"].ToString(),
                            Location = reader["Location"].ToString(),
                            Price = decimal.Parse(reader["Price"].ToString())

                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return trainerInfo;
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
