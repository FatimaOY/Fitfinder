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
using MySql.Data.MySqlClient; // Use MySQL


namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for AdminCreateTrainer2.xaml
    /// </summary>
    public partial class AdminCreateTrainer2 : Page
    {
        private UserInfo currentUser;
        private TrainerInfo currentTrainer;
        private Data _data; // Assuming Data class is already defined
        private LoginPage _loginPage;
        private List<WorkoutType> workoutTypes = new List<WorkoutType>
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
        public AdminCreateTrainer2(string email, string password)
        {
            InitializeComponent();
            lstWorkouts.ItemsSource = workoutTypes;
            _data = new Data(); // Initialize the Data instance
            _loginPage = new LoginPage();
            currentUser = _data.GetUserInformation(email, password);
            currentTrainer = _loginPage.GetTrainerInformation(currentUser.userId);

            // Ensure currentTrainer is set
            if (currentTrainer != null)
            {
                TrainerSession.CurrentTrainer = currentTrainer;
            }
            else
            {
                MessageBox.Show("Error: Trainer information could not be retrieved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void lstWorkouts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        public int GetWorkoutTypeIdByName(string workoutTypeName)
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";
            int workoutTypeId = -1; // Default value if not found
            string query = "SELECT WorkoutTypeId FROM Workouttypes WHERE Name = @WorkoutTypeName";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutTypeName", workoutTypeName);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        workoutTypeId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while getting workout type ID: " + ex.Message);
                }
            }

            return workoutTypeId;
        }

        private void ForwardSelectedWorkoutsToDatabase(List<string> selectedWorkouts)
        {
            if (TrainerSession.CurrentTrainer == null)
            {
                MessageBox.Show("Error: Trainer session is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            int trainerId = TrainerSession.CurrentTrainer.TrainerId;

            try
            {
                connection.Open();

                foreach (string workout in selectedWorkouts)
                {
                    // Perform an SQL INSERT operation for each selected workout
                    string query = "INSERT INTO trainerworkout (PersonalTrainer, WorkoutType) VALUES (@PersonalTrainer, @WorkoutType)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonalTrainer", trainerId);
                    command.Parameters.AddWithValue("@WorkoutType", GetWorkoutTypeIdByName(workout));
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Selected workouts forwarded to the database successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while forwarding selected workouts to the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Questions_confirm_click(object sender, RoutedEventArgs e)
        {
            int currentId = currentUser.userId;

            if (currentTrainer == null)
            {
                MessageBox.Show("Error: Trainer session is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int trainerId = currentTrainer.TrainerId;

            // Retrieve values from UI elements
            string location = txtTrainerLocation.Text;
            decimal price = decimal.Parse(txtTraineePrice.Text); // Assuming price is stored as decimal in database
            List<string> selectedWorkouts = lstWorkouts.SelectedItems
                .Cast<WorkoutType>() // Cast to WorkoutType
                .Select(workout => workout.Name) // Select the Name property
                .ToList(); // Convert to List<string>
            TrainerSession.styles = selectedWorkouts;

            if (selectedWorkouts.Count > 0)
            {
                string selectedWorkoutsMessage = "Selected Workouts: " + string.Join(", ", selectedWorkouts);
                MessageBox.Show(selectedWorkoutsMessage, "Selected Workouts", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No workouts selected.", "Selected Workouts", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Debug output: Print the parameters
            MessageBox.Show($"Debug: TrainerId = {currentId}, Location = {location}, Price = {price}");

            // Construct connection string
            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;";

            // Construct SQL query
            string updateQuery = "UPDATE trainer SET Location = @Location, Price = @Price WHERE TrainerId = @TrainerId";

            try
            {
                // Establish connection
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Open connection
                    connection.Open();

                    // Execute update query
                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@TrainerId", trainerId);

                        // Execute query
                        int rowsAffected = command.ExecuteNonQuery();

                        // Debug output: Print the number of rows affected
                        Console.WriteLine($"Debug: Rows affected = {rowsAffected}");

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            ForwardSelectedWorkoutsToDatabase(selectedWorkouts);
                            // Handle successful update
                            MessageBox.Show("Data updated successfully.");
                            // Navigate to the login page again
                            AdminPageListOfTrainers adminPageListOfTrainers = new AdminPageListOfTrainers();
                            this.NavigationService.Navigate(adminPageListOfTrainers);
                        }
                        else
                        {
                            // Handle no rows affected
                            MessageBox.Show("No data updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
