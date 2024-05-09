using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for TrainerProfile.xaml
    /// </summary>
    public partial class TrainerProfile : Page
    {
        public TrainerProfile()
        {
            InitializeComponent();
        }

        private void Back_button(object sender, RoutedEventArgs e)
        {
            TrainerMainPage trainerMainPage = new TrainerMainPage();
            this.NavigationService.Navigate(trainerMainPage);
        }

        private void UploadProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void change_password_button(object sender, RoutedEventArgs e)
        {
            changePasswordPage changePassword = new changePasswordPage();
            this.NavigationService.Navigate(changePassword);
        }
    }
}
