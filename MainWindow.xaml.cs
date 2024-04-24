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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Add code for the Register button click event
            RegisterPage registerPage = new RegisterPage();
            registerPage.ShowDialog();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Add code for the Login button click event
        }

    }
}
