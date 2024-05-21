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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Page
    {
        private string connectionString =
            "datasource=127.0.0.1;" +
            "port=3306;" +
            "username=root;" +
            "password=;" +
            "database=fitfinder4";

        private string userEmail;
        public ForgotPassword(string email)
        {
            userEmail = email;
            InitializeComponent();
        }

        private void RecoverPassword(object sender, RoutedEventArgs e)
        {
            string answer1 = (txtAnswer1.Text).ToLower().Trim();
            string answer2 = (txtAnswer2.Text).ToLower().Trim();
            string answer3 = (txtAnswer3.Text).ToLower().Trim();
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Verify answers against stored hashed answers
            if (VerifyAnswers(answer1, answer2, answer3))
            {
                if (newPassword == confirmPassword)
                {
                    // Update the password in the database
                    if (UpdatePassword(newPassword))
                    {
                        MessageBox.Show("Your password has been reset successfully.");
                        // Navigate to the login page again
                        LoginPage loginPage = new LoginPage();
                        this.NavigationService.Navigate(loginPage);
                    }
                    else
                    {
                        MessageBox.Show("There was an error updating your password. Please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("The new passwords do not match. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("The answers to the security questions do not match our records. Please try again.");
            }
        }

        private bool VerifyAnswers(string answer1, string answer2, string answer3)
        {
            bool areAnswersValid = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM user WHERE FavoriteColor = @answer1 AND DreamDestination = @answer2 AND FavoriteAnimal = @answer3";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@answer1", answer1);
                    cmd.Parameters.AddWithValue("@answer2", answer2);
                    cmd.Parameters.AddWithValue("@answer3", answer3);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    areAnswersValid = result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while verifying answers: " + ex.Message);
            }

            return areAnswersValid;
        }

        private bool UpdatePassword(string newPassword)
        {
            bool isUpdated = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE user SET Password = @newPassword WHERE FavoriteColor = @answer1 AND DreamDestination = @answer2 AND FavoriteAnimal = @answer3 AND Email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@newPassword", newPassword);
                    cmd.Parameters.AddWithValue("@answer1", txtAnswer1.Text);
                    cmd.Parameters.AddWithValue("@answer2", txtAnswer2.Text);
                    cmd.Parameters.AddWithValue("@answer3", txtAnswer3.Text);
                    cmd.Parameters.AddWithValue("@Email", userEmail);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    isUpdated = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the password: " + ex.Message);
            }

            return isUpdated;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the login page again
            LoginPage loginPage = new LoginPage();
            this.NavigationService.Navigate(loginPage);
        }
    }
}
