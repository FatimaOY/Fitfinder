using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe; // MySQL connection

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for BrowseTrainers.xaml
    /// </summary>
    public partial class BrowseTrainers : Page
    {
        private List<WorkoutType> workoutTypes;

        public BrowseTrainers()
        {
            InitializeComponent();
            genderComboBox.ItemsSource = new List<string> { "Male", "Female", "Other" };
            workoutTypes = new List<WorkoutType>
            {
                new WorkoutType(1, "Weightlifting"),
                new WorkoutType(2, "Cardio"),
                new WorkoutType(3, "Stretching"),
                new WorkoutType(4, "Yoga"),
                new WorkoutType(5, "Pilates"),
                new WorkoutType(6, "CrossFit"),
                new WorkoutType(7, "Calinistics"),
                new WorkoutType(8, "Swimming")
            };

            // Bind the ComboBox to the collection of WorkoutType objects
            workoutTypeComboBox.ItemsSource = workoutTypes;

            // Set the DisplayMemberPath to display the Name property
            workoutTypeComboBox.DisplayMemberPath = "Name";
            List<TrainerBrowse> trainers = GetTrainersFromDatabase();

            TrainersListBox.ItemsSource = trainers;
        }
        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // You can access the selected gender using SelectedItem property
            string selectedGender = genderComboBox.SelectedItem as string;
            if (selectedGender != null)
            {
                // Do something with the selected gender
                MessageBox.Show($"Selected gender: {selectedGender}");
            }
        }
        private void WorkoutTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // You can access the selected workout type using SelectedItem property
            WorkoutType selectedWorkoutType = workoutTypeComboBox.SelectedItem as WorkoutType;
            if (selectedWorkoutType != null)
            {
                // Do something with the selected workout type
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
                string sqlQuery = "SELECT TrainerId, PersonId, Location, Description, Price FROM Trainer"; // Modify query as needed

                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    

                    while (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["PersonId"]);
                        MessageBox.Show("User ID: " + userId);
                        var userInfo = data.GetUserInformationById(userId);
                        TrainerBrowse trainerBrowse = new TrainerBrowse
                        {
                            Name = userInfo.Name,
                            Surname = userInfo.Surname,
                            Email = userInfo.Email,
                            Location = reader["Location"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"])
                        };

                        trainers.Add(trainerBrowse);
                    }

                    reader.Close();
                }
            }

            return trainers;
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

        }
    }
}
