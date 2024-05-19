using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace Fitfinder
{
    public partial class BrowseTrainers : Page
    {
        private TrainerProfile _trainerProfile;
        private List<WorkoutType> workoutTypes;
        private List<TrainerBrowse> allTrainers;

        public BrowseTrainers()
        {
            InitializeComponent();
            _trainerProfile = new TrainerProfile();
            genderComboBox.ItemsSource = new List<string> { "Male", "Female", "Other" };
            workoutTypes = new List<WorkoutType>
            {
                new WorkoutType(1, "Weightlifting"),
                new WorkoutType(2, "Cardio"),
                new WorkoutType(3, "Stretching"),
                new WorkoutType(4, "Yoga"),
                new WorkoutType(5, "Pilates"),
                new WorkoutType(6, "CrossFit"),
                new WorkoutType(7, "Calisthenics"),
                new WorkoutType(8, "Swimming")
            };

            workoutTypeComboBox.ItemsSource = workoutTypes;
            workoutTypeComboBox.DisplayMemberPath = "Name";
            allTrainers = GetTrainersFromDatabase();
            TrainersItemsControl.ItemsSource = allTrainers;
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            // Find the data context of the clicked button which corresponds to the selected trainer
            if (sender is FrameworkElement button && button.DataContext is TrainerBrowse selectedTrainer)
            {
                TrainerDetails trainerDetailsPage = new TrainerDetails(selectedTrainer);
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(trainerDetailsPage);
                }
                else
                {
                    MessageBox.Show("Navigation service is not available.");
                }
            }
        }


        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGender = genderComboBox.SelectedItem as string;
            if (selectedGender != null)
            {
                MessageBox.Show($"Selected gender: {selectedGender}");
            }
        }

        private void WorkoutTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkoutType selectedWorkoutType = workoutTypeComboBox.SelectedItem as WorkoutType;
            if (selectedWorkoutType != null)
            {
                MessageBox.Show($"Selected workout type: {selectedWorkoutType.Name}");
            }
        }

        public class TrainerBrowse
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public List<string> WorkoutTypes { get; set; }
            public string GenderName { get; set; }
            public int GenderId { get; set; }

            //public byte ProfileImage { get; set; }
        }

        public List<TrainerBrowse> GetTrainersFromDatabase()
        {
            Data data = new Data();
            string connectionString = "datasource=127.0.0.1;" +
                                      "port=3306;" +
                                      "username=root;" +
                                      "password=;" +
                                      "database=fitfinder4";

            List<TrainerBrowse> trainers = new List<TrainerBrowse>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sqlQuery = "SELECT TrainerId, PersonId, Location, Description, Price FROM Trainer";

                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["PersonId"]);
                        int trainerId = Convert.ToInt32(reader["TrainerId"]);

                        List<string> stylesList = _trainerProfile.UpdateTrainerStyles(trainerId);
                        var userInfo = data.GetUserInformationById(userId);
                        int genderId = data.GetGenderIdByUserId(userId); // Retrieve the GenderId using the new method
                        string genderName = data.GetGenderName(genderId);

                        TrainerBrowse trainerBrowse = new TrainerBrowse
                        {
                            Name = userInfo.Name,
                            Surname = userInfo.Surname,
                            Email = userInfo.Email,
                            Location = reader["Location"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            WorkoutTypes = stylesList,
                            GenderName = genderName, // Ensure GenderId is set correctly
                            GenderId = genderId,
                            //ProfileImage = Convert.ToByte(LoadProfilePicture(userInfo.Email))
                        };

                        trainers.Add(trainerBrowse);
                    }

                    reader.Close();
                }
            }

            return trainers;
        }

        /*public BitmapImage LoadProfilePicture(string email)
        {
            // Connection string to your database
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=fitfinder4";

            string query = "SELECT ProfilePic FROM user WHERE Email = @Email";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        byte[] imageData = (byte[])result;

                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = ms;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze(); // To make the image thread-safe

                            return bitmapImage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile picture: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null; // Return null if no image was found or an error occurred
        }*/


        private void ApplyFilters_button(object sender, RoutedEventArgs e)
        {
            var filteredTrainers = ApplyFilters();
            DisplayTrainers(filteredTrainers);
        }

        private List<TrainerBrowse> ApplyFilters()
        {
            string location = LocationInput.Text;
            int genderIndex = genderComboBox.SelectedIndex + 1;
            string workoutType = workoutTypeComboBox.SelectedItem != null ? ((WorkoutType)workoutTypeComboBox.SelectedItem).Name : null;
            decimal minPrice = decimal.TryParse(MinPriceInput.Text, out decimal min) ? min : 0;
            decimal maxPrice = decimal.TryParse(MaxPriceInput.Text, out decimal max) ? max : decimal.MaxValue;

            var filteredTrainers = allTrainers.Where(trainer =>
                (string.IsNullOrEmpty(location) || trainer.Location.Contains(location, StringComparison.OrdinalIgnoreCase)) &&
                (genderIndex == 0 || trainer.GenderId == genderIndex) && // Adjust gender filter
                (string.IsNullOrEmpty(workoutType) || trainer.WorkoutTypes.Any(wt => wt.Equals(workoutType, StringComparison.OrdinalIgnoreCase))) &&
                (trainer.Price >= minPrice && trainer.Price <= maxPrice)
            ).ToList();

            return filteredTrainers;
        }
        private void DisplayTrainers(List<TrainerBrowse> trainers)
        {
            TrainersItemsControl.ItemsSource = trainers;
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
            // Handle messages button click
        }
    }
}
