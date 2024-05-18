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
using static Fitfinder.BrowseTrainers;
using MySql.Data.MySqlClient;



namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for ScheduleWorkout.xaml
    /// </summary>
    public partial class ScheduleWorkout : Page
    {
        private string trainerEmail;

        public ScheduleWorkout(string email)
        {
            InitializeComponent();
            trainerEmail = email;
            if (!string.IsNullOrEmpty(trainerEmail))
            {
                LoadAvailabilities(trainerEmail);
            }
            else
            {
                MessageBox.Show("Trainer information is not available.");
            }
        }

        public class TrainerBrowse
        {
            public int TrainerId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public List<string> WorkoutTypes { get; set; }
        }

        public int GetUserId(string email)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int userId = -1; // Default value indicating user not found

            string query = "SELECT UserId FROM user WHERE Email = @Email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return userId;
        }
        private void LoadAvailabilities(string email)
        {
            Data data = new Data(); // Create an instance of the Data class
            int trainerID = data.GetTrainerID(GetUserId(email));
            MessageBox.Show(trainerID.ToString());
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM availability WHERE TrainerId = @TrainerId";
                    cmd.Parameters.AddWithValue("@TrainerId", trainerID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Availability availability = new Availability(
                                availabilityId: Convert.ToInt32(reader["AvailabilityId"]),
                                trainerId: Convert.ToInt32(reader["TrainerId"]),
                                day: reader["Day"].ToString(),
                                startTime: TimeSpan.Parse(reader["StartTime"].ToString()),
                                endTime: TimeSpan.Parse(reader["EndTime"].ToString()));

                            UpdateUIWithAvailability(availability);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UpdateUIWithAvailability(Availability availability)
        {
            // Update the UI elements to reflect the trainer's availability
            // For example, highlight available time slots in the ListBox
            switch (availability.Day.ToLower())
            {
                case "monday":
                    MondayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                case "tuesday":
                    TuesdayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                // Repeat for other days of the week
                case "wednesday":
                    WednesdayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                case "thursday":
                    ThursdayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                case "friday":
                    FridayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                case "saturday":
                    SaturdayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
                case "sunday":
                    SundayTimeSlots.Items.Add(new ListBoxItem { Content = $"{availability.StartTime} - {availability.EndTime}" });
                    break;
            }
        }

        private void TimeSlot_Click(object sender, SelectionChangedEventArgs e)
        {
            //
           
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
            MessagesTrainee messagesTrainee = new MessagesTrainee();
            this.NavigationService.Navigate(messagesTrainee);
        }
    }
}
