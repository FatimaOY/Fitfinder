using Microsoft.Win32;
using Mysqlx.Crud;
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
using System.IO;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for AdminCreateTrainer.xaml
    /// </summary>
    public partial class AdminCreateTrainer : Page
    {
        Data data = new Data(); // Create an instance of the Data class
        RegisterWindom registerWindow = new RegisterWindom(); // Create an instance of the Data class
        private MyViewModel _viewModel; // Declare the ViewModel
        public AdminCreateTrainer()
        {
            InitializeComponent();
            _viewModel = new MyViewModel(); // Initialize the ViewModel

        }


        private byte[] imageData; // Declare imageData at the class level
                                  // Method to determine the gender based on the selected radio button
        private int GetGenderIdTrainer()
        {
            if (rbFemale.IsChecked == true) return 1; // Female
            if (rbMale.IsChecked == true) return 2; // Male
            if (rbOther.IsChecked == true) return 3; // Other
            return 0; // Default or unknown
        }

        private void Trainer_final_register_click(object sender, RoutedEventArgs e)
        {
            string name = txtTrainerName.Text;
            string surname = txtTrainerSurname.Text;
            string email = txtTrainerEmail.Text;
            string password = txtTrainerPassword.Password;
            string confirmPassword = txtTrainerConfirmPassword.Password;
            string favoriteColor = (txtTrainerAnswer1.Text).ToLower().Trim();
            string dreamDestination = (txtTrianerAnswer2.Text).ToLower().Trim();
            string favoriteAnimal = (txtTrainerAnswer3.Text).ToLower().Trim();
            /*string location = txtTrainerLocation.Text; // ADD HERE GENDER ID WHEN READY
            int price = Convert.ToInt32(txtTraineePrice.Text);*/

            // Validate input (check if passwords match)
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            if (registerWindow.IfEmailExists(email) == true)
            {
                MessageBox.Show("An account with this email already exists.");
                return;
            }

            int genderId = GetGenderIdTrainer();

            // Ensure imageData is not null
            if (imageData == null)
            {
                MessageBox.Show("Please upload a profile picture.");
                return;
            }

            // Create a Trainer object
            Trainer trainer = new Trainer(0, name, surname, email, password, imageData, genderId, null, null, 0, favoriteColor, dreamDestination, favoriteAnimal);


            // Insert into the database using the ViewModel
            try
            {
                _viewModel.AddNewTrainer(trainer); // Adding a trainer
                MessageBox.Show("Trainer registered successfully.");


                AdminCreateTrainer2 adminCreateTrainer2 = new AdminCreateTrainer2(email, password);
                this.NavigationService.Navigate(adminCreateTrainer2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Handle exceptions
            }
        }

        private void UploadProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter for file extension and default file extension
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = openFileDialog.ShowDialog();

            // Get the selected file name and display in an Image control
            if (result == true)
            {
                // Open document
                string filename = openFileDialog.FileName;

                // Display selected image in the Image control
                ProfilePicture.Source = new BitmapImage(new Uri(filename));

                // Convert the selected image file to byte array
                imageData = File.ReadAllBytes(filename);
            }
        }
    }
}
