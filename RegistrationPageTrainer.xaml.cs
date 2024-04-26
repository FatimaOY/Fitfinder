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
    /// Interaction logic for RegistrationPageTrainer.xaml
    /// </summary>
    public partial class RegistrationPageTrainer : Page
    {
        public RegistrationPageTrainer()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get trainer registration information
            string location = txtTraineeLocation.Text;
            string gender = "";
            if (rbFemale.IsChecked == true)
                gender = "Female";
            else if (rbMale.IsChecked == true)
                gender = "Male";
            else if (rbOther.IsChecked == true)
                gender = "Other";
            else
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            int price;
            if (!int.TryParse(txtTraineePrice.Text, out price))
            {
                MessageBox.Show("Price must be a valid integer.");
                return;
            }
            string workouts = "";
            foreach (ListBoxItem item in lstWorkouts.SelectedItems)
            {
                workouts += item.Content.ToString() + ", ";
            }
            if (workouts.Length > 0)
            {
                workouts = workouts.Substring(0, workouts.Length - 2); // Remove trailing comma and space
            }
            else
            {
                MessageBox.Show("Please select at least one workout type.");
                return;
            }

            // Process trainer registration (e.g., save to database)
            // Here, you would typically handle the registration logic
            // For now, let's just display a message
            MessageBox.Show($"Trainer Registration\nLocation: {location}\nGender: {gender}\nPrice: {price}\nWorkouts: {workouts}");
        }

    }
}
