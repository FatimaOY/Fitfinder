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
    /// Interaction logic for RegisterWindom.xaml
    /// </summary>
    public partial class RegisterWindom : Page
    {
        public RegisterWindom()
        {
            InitializeComponent();
        }

        private void RegisterTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            // Show trainee registration form and hide trainer registration form
            TraineeRegistrationForm.Visibility = Visibility.Visible;
            TrainerRegistrationForm.Visibility = Visibility.Collapsed;
        }

        private void RegisterTrainerButton_Click(object sender, RoutedEventArgs e)
        {
            // Show trainer registration form and hide trainee registration form
            TrainerRegistrationForm.Visibility = Visibility.Visible;
            TraineeRegistrationForm.Visibility = Visibility.Collapsed;
        }

        private void TraineeRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get trainee registration information
            string name = txtTraineeName.Text;
            string surname = txtTraineeSurname.Text;
            string password = txtTraineePassword.Password;
            string confirmPassword = txtTraineeConfirmPassword.Password;

            // Validate input (for example, check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Process trainee registration (e.g., save to database)
            // Here, you would typically handle the registration logic
            // For now, let's just display a message
            MessageBox.Show($"Trainee Registration\nName: {name}\nSurname: {surname}\nPassword: {password}");
        }

        private void TrainerNextButton_Click(object sender, RoutedEventArgs e)
        {
            // Get trainer registration information
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;

            // Validate input (for example, check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Process trainer registration (e.g., save to database)
            // Here, you would typically handle the registration logic
            // For now, let's just display a message
            MessageBox.Show($"Trainer Registration\nName: {name}\nSurname: {surname}\nPassword: {password}");

            // Navigate to the next step (e.g., additional trainer registration information)
            // For demonstration purposes, let's assume there's another page for additional registration steps
            // Replace "NextPage" with the actual name of the page
            // NavigationService.Navigate(new NextPage());
        }
    }
}