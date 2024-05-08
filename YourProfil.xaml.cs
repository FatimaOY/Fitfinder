﻿using System;
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
    /// Interaction logic for YourProfil.xaml
    /// </summary>
    public partial class YourProfil : Page
    {
        public YourProfil()
        {
            InitializeComponent();
            LoadUserProfile();
        }
        private void LoadUserProfile()
        {
            var currentUser = UserSession.CurrentUser;

            if (currentUser != null)
            {
                NameTextBlock.Text = $"Name: {currentUser.FirstName}";
                SurnameTextBlock.Text = $"Surname: {currentUser.Surname}";
                EmailTextBlock.Text = $"Email: {currentUser.Email}";
                string savedEmail = currentUser.Email;
                // If you have a password field, it's usually a placeholder for user interaction (e.g., change password)
                // Avoid displaying plain-text passwords
                PasswordTextBlock.Text = "Password: *********"; // Masked for security
            }
            else
            {
                MessageBox.Show("User information could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Profile_button(object sender, RoutedEventArgs e)
        {
            YourProfil yourProfil = new YourProfil();
            this.NavigationService.Navigate(yourProfil);
        }

        private void BrowseTrainer_button(object sender, RoutedEventArgs e)
        {
            BrowseTrainers browseTrainers = new BrowseTrainers();
            this.NavigationService.Navigate(browseTrainers);
        }

        private void YourWorkouts_button(object sender, RoutedEventArgs e)
        {
            YourWorkouts yourWorkouts = new YourWorkouts();
            this.NavigationService.Navigate(yourWorkouts);
        }

        private void Messages_button(object sender, RoutedEventArgs e)
        {
            MessagesTrainee messagesTrainee = new MessagesTrainee();
            this.NavigationService.Navigate(messagesTrainee);
        }

        private void change_password_button(object sender, RoutedEventArgs e)
        {
            changePasswordPage changepasswordPage = new changePasswordPage();
            this.NavigationService.Navigate(changepasswordPage);
        }
    }
}
