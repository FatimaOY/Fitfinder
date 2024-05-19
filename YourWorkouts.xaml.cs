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

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for YourWorkouts.xaml
    /// </summary>
    public partial class YourWorkouts : Page
    {
        private string connectionString =
            "datasource=127.0.0.1;" +
            "port=3306;" +
            "username=root;" +
            "password=;" +
            "database=fitfinder4";
        public YourWorkouts()
        {
            InitializeComponent();
            PopulateAppointmentsListView();
        }

        public class AppointmentViewModel
        {
            public string Trainer { get; set; }
            public string WorkoutType { get; set; }
            public string Duration { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Status { get; set; }
        }
        private void PopulateAppointmentsListView()
        {
            List<AppointmentViewModel> appointments = GetAppointmentsFromDatabase();
            userListView.ItemsSource = appointments;
        }

        private List<AppointmentViewModel> GetAppointmentsFromDatabase()
        {
            Data data = new Data(); // Create an instance of the Data class

            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            var currentUser = UserSession.CurrentUser; // Assuming this retrieves the current user session info

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Your SQL query to fetch appointments
                    string query = @"
                    SELECT 
                        u.Name AS TrainerName,
                        u.Surname AS TrainerSurname,
                        wt.Name AS WorkoutType, 
                        ap.Duration, 
                        ap.Date, 
                        ap.Status 
                    FROM appointment AS ap 
                    JOIN trainerworkout AS tw ON ap.TrainerWorkoutId = tw.TrainerWorkoutId 
                    JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId 
                    JOIN trainer AS t ON ap.TrainerId = t.TrainerId
                    JOIN user AS u ON t.PersonId = u.UserID
                    WHERE ap.TraineeId = @TraineeId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TraineeId", data.GetTraineeID(currentUser.userId));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string trainer = $"{reader["TrainerName"]} {reader["TrainerSurname"]}";
                            string workoutType = reader["WorkoutType"].ToString();
                            string duration = "1 hour".ToString();
                            DateTime date = Convert.ToDateTime(reader["Date"]);
                            string status = reader["Status"].ToString();

                            appointments.Add(new AppointmentViewModel
                            {
                                Trainer = trainer,
                                WorkoutType = workoutType,
                                Duration = duration,
                                Date = date.ToShortDateString(),
                                Time = date.ToShortTimeString(),
                                Status = status
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return appointments;
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
