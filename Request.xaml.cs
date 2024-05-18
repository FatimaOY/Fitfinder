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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Fitfinder
{
    public partial class Request : Page
    {
        public Request()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            // Sample data - replace with your actual data source
            var requests = new List<RequestModel>
            {
                new RequestModel { Name = "John", Surname = "Doe", Gender = "Male", WorkoutStyle = "Cardio", Email = "john.doe@example.com" },
                new RequestModel { Name = "Jane", Surname = "Smith", Gender = "Female", WorkoutStyle = "Strength", Email = "jane.smith@example.com" }
            };

            RequestsDataGrid.ItemsSource = requests;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as RequestModel;
            MessageBox.Show($"Accepted request from {request.Name} {request.Surname}");
            // Add logic to handle acceptance
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as RequestModel;
            MessageBox.Show($"Declined request from {request.Name} {request.Surname}");
            // Add logic to handle decline
        }
    }

    public class RequestModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string WorkoutStyle { get; set; }
        public string Email { get; set; }
    }
}
