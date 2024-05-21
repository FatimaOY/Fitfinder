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
    /// Interaction logic for YourWorkoutsTrainer.xaml
    /// </summary>
    public partial class YourWorkoutsTrainer : Page
    {
        private string connectionString =
            "datasource=127.0.0.1;" +
            "port=3306;" +
            "username=root;" +
            "password=;" +
            "database=fitfinder4";
        public YourWorkoutsTrainer()
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
                    string query = @"SELECT u.Name AS TrainerName, u.Surname AS TrainerSurname, wt.Name AS WorkoutType, ap.Duration, ap.Date, ap.Status FROM appointment AS ap JOIN trainerworkout AS tw ON ap.TrainerWorkoutId = tw.TrainerWorkoutId JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId JOIN client AS t ON ap.TraineeId = t.TraineeId JOIN user AS u ON t.PersonId = u.UserID WHERE ap.TrainerId = @TrainerId;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetTrainerID(currentUser.userId));

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
            TrainerProfile yourProfil = new TrainerProfile();
            this.NavigationService.Navigate(yourProfil);
        }

        private void Requests_button(object sender, RoutedEventArgs e)
        {
            Request request = new Request();
            this.NavigationService.Navigate(request);
        }

        private void YourWorkouts_button(object sender, RoutedEventArgs e)
        {
            YourWorkoutsTrainer yourWorkoutsTrainer = new YourWorkoutsTrainer();
            this.NavigationService.Navigate(yourWorkoutsTrainer);
        }

        private void Calendar_button(object sender, RoutedEventArgs e)
        {
            // Show the week selection window
            WeekSelectionWindow weekSelection = new WeekSelectionWindow();
            if (weekSelection.ShowDialog() == true)
            {
                // Get the selected week number from the WeekSelection instance
                int selectedWeek = weekSelection.SelectedWeek;

                // Pass the selected week number to the CalendarTrainer constructor
                CalendarTrainer calendarTrainer = new CalendarTrainer(selectedWeek);
                this.NavigationService.Navigate(calendarTrainer);
            }
        }
    } 
}
