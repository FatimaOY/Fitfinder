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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
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
            // Add code to validate and navigate to the next page
            MessageBox.Show("Login successful!", "Login");
            // Example: Navigate to another page after successful login
            // NavigationService.Navigate(new NextPage());
        }
    }
}
