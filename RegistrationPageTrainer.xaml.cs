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
        private Data database = new Data();

        public RegistrationPageTrainer()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get trainer registration information
            string location = txtTraineeLocation.Text;
            int genderId = 0; // Initialize with 0 which means no gender selected
            if (rbFemale.IsChecked == true)
                genderId = 1; // Female
            else if (rbMale.IsChecked == true)
                genderId = 2; // Male
            else if (rbOther.IsChecked == true)
                genderId = 3; // Other
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
            try
            {
                // Assuming you have a method to insert a trainer
                //database.InsertTrainer(new Trainer(location, genderId, price, workouts));
                //MessageBox.Show("Trainer registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            // Process trainer registration (e.g., save to database)
            // Here, you would typically handle the registration logic
            // For now, let's just display a message
            MessageBox.Show($"Trainer Registration\nLocation: {location}\nGender: {genderId}\nPrice: {price}\nWorkouts: {workouts}");
        }

    }
}
