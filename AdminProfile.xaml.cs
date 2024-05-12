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

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for AdminProfile.xaml
    /// </summary>
    public partial class AdminProfile : Page
    {
        public AdminProfile()
        {
            InitializeComponent();
            LoadUserProfile();
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

        private void change_password_button(object sender, RoutedEventArgs e)
        {
            changePasswordPage ChangePasswordPage = new changePasswordPage();
            this.NavigationService.Navigate(ChangePasswordPage);
        }

        private void Back_button(object sender, RoutedEventArgs e)
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            this.NavigationService.Navigate(adminMainPage);
        }
    }
}
