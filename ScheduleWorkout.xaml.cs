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
        private int selectedWeek;
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=fitfinder4";

        public ScheduleWorkout(string email, int week)
        {
            InitializeComponent();
            trainerEmail = email;
            selectedWeek = week;
            InitializeTimeSlots();
            PopulateWorkoutTypes();
            if (!string.IsNullOrEmpty(trainerEmail))
            {
                LoadAvailabilities(trainerEmail);
            }
            else
            {
                MessageBox.Show("Trainer information is not available.");
            }
        }

        private void PopulateWorkoutTypes()
        {
            List<string> workoutTypes = GetTrainerWorkoutTypes();
            foreach (string workoutType in workoutTypes)
            {
                WorkoutTypesComboBox.Items.Add(workoutType);
            }
        }



        private void WorkoutTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                // Get the selected item from the ComboBox
                string selectedWorkoutType = comboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedWorkoutType))
                {
                    // Add your logic here based on the selected workout type
                    // For example, you can display additional information or perform some action
                    MessageBox.Show($"Selected workout type: {selectedWorkoutType}");
                }
            }
        }

        private void InitializeTimeSlots()
        {
            CreateTimeSlots(MondayTimeSlots, "Monday");
            CreateTimeSlots(TuesdayTimeSlots, "Tuesday");
            CreateTimeSlots(WednesdayTimeSlots, "Wednesday");
            CreateTimeSlots(ThursdayTimeSlots, "Thursday");
            CreateTimeSlots(FridayTimeSlots, "Friday");
            CreateTimeSlots(SaturdayTimeSlots, "Saturday");
            CreateTimeSlots(SundayTimeSlots, "Sunday");
        }

        private void CreateTimeSlots(ListBox listBox, string dayOfWeek)
        {
            for (int hour = 8; hour <= 19; hour++)
            {
                string timeSlot = $"{hour}:00 - {hour + 1}:00";
                ListBoxItem item = new ListBoxItem { Content = timeSlot, Tag = dayOfWeek };
                listBox.Items.Add(item);
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
            Data data = new Data();
            int trainerID = data.GetTrainerID(GetUserId(email));
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM availability WHERE TrainerId = @TrainerId AND WeekNumber = @WeekNumber";
                    cmd.Parameters.AddWithValue("@TrainerId", trainerID);
                    cmd.Parameters.AddWithValue("@WeekNumber", selectedWeek);

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

                            HighlightAvailability(availability);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private Dictionary<string, List<TimeSpan>> availableSlots = new Dictionary<string, List<TimeSpan>>();

        private void HighlightAvailability(Availability availability)
        {
            ListBox listBox = GetListBoxForDay(availability.Day);
            if (listBox != null && listBox.Items != null)
            {
                foreach (ListBoxItem item in listBox.Items)
                {
                    if (item.Content != null)
                    {
                        string[] times = item.Content.ToString().Split('-');
                        if (times.Length == 2)
                        {
                            TimeSpan start = TimeSpan.Parse(times[0].Trim());
                            TimeSpan end = TimeSpan.Parse(times[1].Trim());
                            if (availability.StartTime <= start && availability.EndTime >= end)
                            {
                                item.Background = new SolidColorBrush(Colors.LightBlue);

                                if (!availableSlots.ContainsKey(availability.Day))
                                {
                                    availableSlots[availability.Day] = new List<TimeSpan>();
                                }
                                availableSlots[availability.Day].Add(start);
                            }
                        }
                    }
                }
            }
        }


        private ListBox GetListBoxForDay(string day)
        {
            switch (day.ToLower())
            {
                case "monday":
                    return MondayTimeSlots;
                case "tuesday":
                    return TuesdayTimeSlots;
                case "wednesday":
                    return WednesdayTimeSlots;
                case "thursday":
                    return ThursdayTimeSlots;
                case "friday":
                    return FridayTimeSlots;
                case "saturday":
                    return SaturdayTimeSlots;
                case "sunday":
                    return SundayTimeSlots;
                default:
                    return null;
            }
        }

        private void TimeSlot_Click(object sender, RoutedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedItem != null && listBox.SelectedItem is ListBoxItem listBoxItem)
            {
                string dayOfWeek = listBoxItem.Tag as string;
                string selectedWorkout = WorkoutTypesComboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(dayOfWeek) && !string.IsNullOrEmpty(selectedWorkout))
                {
                    string timeSlot = listBoxItem.Content.ToString();
                    MessageBoxResult result = MessageBox.Show("Do you want to schedule a workout here?", "Schedule Workout", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        SaveAppointment(dayOfWeek, timeSlot, selectedWorkout, selectedWeek);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a workout type.");
                }
            }
        }


        private List<string> GetTrainerWorkoutTypes()
        {
            List<string> workoutTypes = new List<string>();
            Data data = new Data();
            int trainerID = data.GetTrainerID(GetUserId(trainerEmail));

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT wt.Name FROM `trainerworkout` as tw JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId WHERE tw.PersonalTrainer = @PersonalTrainer";
                    cmd.Parameters.AddWithValue("@PersonalTrainer", trainerID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            workoutTypes.Add(reader["Name"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return workoutTypes;
        }

        private void SaveAppointment(string dayOfWeek, string timeSlot, string selectedWorkout, int selectedWeek)
        {
            // Check if the selected time slot is available
            if (!availableSlots.ContainsKey(dayOfWeek) || !availableSlots[dayOfWeek].Any(t => t.ToString(@"hh\:mm") == timeSlot.Split('-')[0].Trim()))
            {
                MessageBox.Show("The selected time slot is not available. Please choose a highlighted time slot.");
                return;
            }

            Data data = new Data();
            var currentUser = UserSession.CurrentUser;
            int traineeID = data.GetTraineeID(data.GetUserId(currentUser.Email, currentUser.Password));
            int trainerID = data.GetTrainerID(GetUserId(trainerEmail));
            int trainerWorkoutID = GetTrainerWorkoutId(selectedWorkout);

            // Extract start time from the time slot
            string[] times = timeSlot.Split('-');
            if (times.Length != 2)
            {
                MessageBox.Show("Invalid time slot format.");
                return;
            }

            // Parse the start time
            TimeSpan startTime = TimeSpan.Parse(times[0].Trim());

            // Combine date and time
            DateTime appointmentDate = GetNextDayOfWeek(dayOfWeek).Add(startTime);

            string status = "Pending";
            TimeSpan duration = TimeSpan.FromHours(1);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO appointment (TraineeId, TrainerId, TrainerWorkoutId, Duration, Date, Status, WeekNumber) VALUES (@TraineeId, @TrainerId, @TrainerWorkoutId, @Duration, @Date, @Status, @WeekNumber)";
                    cmd.Parameters.AddWithValue("@TraineeId", traineeID);
                    cmd.Parameters.AddWithValue("@TrainerId", trainerID);
                    cmd.Parameters.AddWithValue("@TrainerWorkoutId", trainerWorkoutID);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@Date", appointmentDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@WeekNumber", selectedWeek);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Appointment scheduled successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private int GetTrainerWorkoutId(string workoutName)
        {
            int workoutId = -1;
            Data data = new Data();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT tw.TrainerworkoutId FROM trainerworkout AS tw JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId WHERE wt.Name = @WorkoutName AND tw.PersonalTrainer = @PersonalTrainer";
                    cmd.Parameters.AddWithValue("@WorkoutName", workoutName);
                    int trainerID = data.GetTrainerID(GetUserId(trainerEmail));
                    cmd.Parameters.AddWithValue("@PersonalTrainer", trainerID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            workoutId = Convert.ToInt32(reader["TrainerworkoutId"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return workoutId;
        }

        private DateTime GetNextDayOfWeek(string dayOfWeek)
        {
            DayOfWeek desiredDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek, true);
            DateTime today = DateTime.Today;
            int daysUntilNextDay = ((int)desiredDay - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilNextDay);
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