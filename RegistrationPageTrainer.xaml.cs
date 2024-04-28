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
    public partial class RegistrationPageTrainer : Page
    {
        private string name;
        private string surname;
        private string email;
        private string password;
        private MyViewModel _viewModel; // Declare _viewModel here

        public RegistrationPageTrainer(string name, string surname, string email, string password, MyViewModel viewModel)
        {
            InitializeComponent();

            // Store the trainer information received from the first page
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;

            _viewModel = viewModel; // Assign the ViewModel
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get additional trainer registration information from the second page
            string location = txtTrainerLocation.Text;
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

            // Create a Trainer object using all information
            Trainer trainer = new Trainer(0, name, surname, email, password, null, genderId, "Trainer description", location, price, 5, "Trainer certifications", true);

            try
            {
                _viewModel.AddNewTrainer(trainer); // Adding a trainer
                MessageBox.Show("Trainer registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }
    }
}