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
    /// Interaction logic for changePasswordPage.xaml
    /// </summary>
    public partial class changePasswordPage : Page
    {
        public changePasswordPage()
        {
            InitializeComponent();
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve entered passwords from the PasswordBoxes
            string currentPassword = txtCurrentPassword.Password;
            string newPassword = txtNewPassword.Password;
            string confirmNewPassword = txtConfirmNewPassword.Password;

            // Check if new password and confirm password match
            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("New password and confirm password do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Here you can implement the logic to update the password in the database
            // For demonstration purposes, let's just display a message
            MessageBox.Show("Password updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // You can also navigate to another page after updating the password if needed
            YourProfil yourProfil = new YourProfil();
            this.NavigationService.Navigate(yourProfil);
        }
    }
}
