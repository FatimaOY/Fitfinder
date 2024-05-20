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
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Fitfinder
{
    public partial class Request : Page
    {
        private int userId;
        private int selectedTraineeId; // Add a field to store the selected trainee ID

        public Request()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            string connectionString = "datasource=127.0.0.1;" +
                                      "port=3306;" +
                                      "username=root;" +
                                      "password=;" +
                                      "database=fitfinder4";

            string query = "SELECT AppointmentId, TraineeId, TrainerWorkoutId, Date, Duration FROM Appointment WHERE Status = 'Pending'";

            var requests = new List<RequestModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int appointmentId = reader.GetInt32("AppointmentId");
                        int traineeId = reader.GetInt32("TraineeId");
                        userId = GetPersonIdFromTraineeId(traineeId);
                        int trainerWorkoutId = reader.GetInt32("TrainerWorkoutId");
                        int workoutTypeId = GetWorkoutType(trainerWorkoutId);
                        string workoutName = GetWorkoutTypeNameById(workoutTypeId);
                        var userInfo = GetUserInformationById(userId);
                        int? genderId = GetGenderIdByUserId(userId);

                        // Retrieve Date and Duration from database
                        DateTime date = reader.GetDateTime("Date");
                        TimeSpan duration = reader.GetTimeSpan("Duration");

                        requests.Add(new RequestModel
                        {
                            AppointmentId = appointmentId,
                            TraineeId = traineeId,
                            Name = userInfo.Name,
                            Surname = userInfo.Surname,
                            Gender = GetGenderName(genderId),
                            WorkoutStyle = workoutName,
                            Email = userInfo.Email,
                            Date = date,
                            Duration = duration
                        });
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            RequestsDataGrid.ItemsSource = requests;
        }

        public int? GetGenderIdByUserId(int userId)
        {
            int? genderId = null;

            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            string query = "SELECT GenderId FROM User WHERE UserID = @UserID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        genderId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return genderId;
        }

        public string GetGenderName(int? genderId)
        {
            if (genderId.HasValue)
            {
                return genderId switch
                {
                    1 => "Male",
                    2 => "Female",
                    3 => "Other",
                    _ => "Unknown"
                };
            }
            else
            {
                return "Unknown"; // or any default value you prefer
            }
        }



        public string GetWorkoutTypeNameById(int workoutTypeId)
        {
            string workoutTypeName = null;

            string connectionString = "Server=127.0.0.1;Port=3306;Database=fitfinder4;Uid=root;Pwd=;"; // Replace this with your actual connection string
            string query = "SELECT Name FROM Workouttypes WHERE WorkoutTypeId = @WorkoutTypeId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutTypeId", workoutTypeId);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        workoutTypeName = Convert.ToString(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while getting workout type name: " + ex.Message);
                }
            }

            return workoutTypeName;
        }
        public UserInfo GetUserInformationById(int userId)
        {
            UserInfo userInfo = null;
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            string query = "SELECT * FROM User WHERE UserID = @UserID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userInfo = new UserInfo
                        {
                            userId = (int)reader["UserID"],
                            Email = reader["Email"].ToString(),
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            Password = reader["Password"].ToString(),
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return userInfo;
        }
        public int GetWorkoutType(int trainerWorkoutId)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int workoutType = -1; // Default value indicating no workout type found

            string query = "SELECT WorkoutType FROM trainerworkout WHERE TrainerWorkoutId = @TrainerWorkoutId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TrainerWorkoutId", trainerWorkoutId);

                try
                {
                    connection.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        workoutType = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return workoutType;
        }


        public int GetPersonIdFromTraineeId(int traineeId)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int personId = -1; // Default value indicating user not found

            string query = "SELECT PersonId FROM client WHERE TraineeId = @TraineeId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TraineeId", traineeId);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        personId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return personId;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var request = button.DataContext as RequestModel;
                if (request != null)
                {
                    MessageBox.Show($"Accepted request from {request.Name} {request.Surname}");
                    // Update the status to "Accepted" in the database
                    UpdateStatus(request.AppointmentId, "Accepted");
                }
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var request = button.DataContext as RequestModel;
                if (request != null)
                {
                    MessageBox.Show($"Declined request from {request.Name} {request.Surname}");
                    // Update the status to "Declined" in the database
                    UpdateStatus(request.AppointmentId, "Declined");
                }
            }
        }


        // Modify UpdateStatus to accept TraineeId directly
        private void UpdateStatus(int appointmentId, string status)
        {
            string connectionString = "datasource=127.0.0.1;" +
                                       "port=3306;" +
                                       "username=root;" +
                                       "password=;" +
                                       "database=fitfinder4";

            string query = "UPDATE Appointment SET Status = @Status WHERE AppointmentId = @AppointmentId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                try
                {
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Status updated successfully
                        LoadRequests(); // Reload requests to reflect updated status
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }

        public class RequestModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int AppointmentId { get; set; } // Add AppointmentId property
        public int TraineeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string WorkoutStyle { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}