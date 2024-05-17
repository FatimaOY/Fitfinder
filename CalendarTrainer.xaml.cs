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
using MySql.Data.MySqlClient; // Add this namespace for MySQL database connectivity


namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for CalendarTrainer.xaml
    /// </summary>
    public partial class CalendarTrainer : Page
    {
        // Mock database for demonstration purposes
        private List<Availability> availabilities = new List<Availability>();
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=fitfinder4";

        public CalendarTrainer()
        {
            InitializeComponent();
            LoadAvailabilities(); // Load availabilities when the page is loaded
        }

        private void TimeSlot_Click(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItems != null)
            {
                // Create a copy of the selected items
                var selectedItemsCopy = new List<ListBoxItem>(listBox.SelectedItems.OfType<ListBoxItem>());

                foreach (var selectedItem in selectedItemsCopy)
                {
                    if (selectedItem != null)
                    {
                        if (selectedItem.Foreground == Brushes.Blue) // Time slot is already selected
                        {
                            // Prompt the user to confirm removal of the availability
                            MessageBoxResult result = MessageBox.Show("Do you want to remove this availability?", "Remove Availability", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                RemoveAvailability(listBox, selectedItem); // Remove availability from the database
                                selectedItem.Foreground = Brushes.Black; // Reset the foreground color
                                selectedItem.IsSelected = false; // Deselect the item
                            }
                        }
                        else
                        {
                            selectedItem.Foreground = Brushes.Blue; // Change foreground color to blue
                            SaveAvailability(listBox, selectedItem); // Save availability to the database
                        }
                    }
                }
            }
        }


        private void SaveAvailability(ListBox listBox, ListBoxItem selectedItem)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    var currentUser = UserSession.CurrentUser;
                    Data data = new Data(); // Create an instance of the Data class
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM availability WHERE TrainerId = @TrainerId AND Day = @Day AND StartTime = @StartTime AND EndTime = @EndTime";
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetTrainerID(data.GetUserId(currentUser.Email, currentUser.Password)));
                    cmd.Parameters.AddWithValue("@Day", GetDayOfWeek(listBox));

                    string[] times = selectedItem.Content.ToString().Split('-');
                    string startTimeString = times[0].Trim();
                    string endTimeString = times[1].Trim();

                    cmd.Parameters.AddWithValue("@StartTime", startTimeString);
                    cmd.Parameters.AddWithValue("@EndTime", endTimeString);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        // If the entry does not exist, insert it
                        cmd.CommandText = "INSERT INTO availability (TrainerId, Day, StartTime, EndTime) VALUES (@TrainerId, @Day, @StartTime, @EndTime)";
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void RemoveAvailability(ListBox listBox, ListBoxItem selectedItem)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    var currentUser = UserSession.CurrentUser;
                    Data data = new Data(); // Create an instance of the Data class
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM availability WHERE TrainerId = @TrainerId AND Day = @Day AND StartTime = @StartTime AND EndTime = @EndTime";
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetTrainerID(data.GetUserId(currentUser.Email, currentUser.Password)));
                    cmd.Parameters.AddWithValue("@Day", GetDayOfWeek(listBox));

                    string[] times = selectedItem.Content.ToString().Split('-');
                    string startTimeString = times[0].Trim();
                    string endTimeString = times[1].Trim();

                    cmd.Parameters.AddWithValue("@StartTime", startTimeString);
                    cmd.Parameters.AddWithValue("@EndTime", endTimeString);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadAvailabilities()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    var currentUser = UserSession.CurrentUser;
                    Data data = new Data(); // Create an instance of the Data class
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM availability WHERE TrainerId = @TrainerId";
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetTrainerID(data.GetUserId(currentUser.Email, currentUser.Password)));
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

                            availabilities.Add(availability);

                            // Update UI
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
            ListBox listBox = GetListBoxForDay(availability.Day);
            if (listBox != null)
            {
                foreach (ListBoxItem item in listBox.Items)
                {
                    string[] times = item.Content.ToString().Split('-');
                    TimeSpan startTime = TimeSpan.Parse(times[0].Trim());
                    TimeSpan endTime = TimeSpan.Parse(times[1].Trim());

                    if (availability.StartTime == startTime && availability.EndTime == endTime)
                    {
                        item.IsSelected = true;
                        item.Foreground = Brushes.Blue;
                    }
                }
            }
        }

        private ListBox GetListBoxForDay(string day)
        {
            switch (day)
            {
                case "Monday":
                    return MondayTimeSlots;
                case "Tuesday":
                    return TuesdayTimeSlots;
                case "Wednesday":
                    return WednesdayTimeSlots;
                case "Thursday":
                    return ThursdayTimeSlots;
                case "Friday":
                    return FridayTimeSlots;
                case "Saturday":
                    return SaturdayTimeSlots;
                case "Sunday":
                    return SundayTimeSlots;
                default:
                    return null;
            }
        }

        private string GetDayOfWeek(ListBox listBox)
        {
            if (listBox == MondayTimeSlots)
                return DayOfWeek.Monday.ToString();
            else if (listBox == TuesdayTimeSlots)
                return DayOfWeek.Tuesday.ToString();
            else if (listBox == WednesdayTimeSlots)
                return DayOfWeek.Wednesday.ToString();
            else if (listBox == ThursdayTimeSlots)
                return DayOfWeek.Thursday.ToString();
            else if (listBox == FridayTimeSlots)
                return DayOfWeek.Friday.ToString();
            else if (listBox == SaturdayTimeSlots)
                return DayOfWeek.Saturday.ToString();
            else if (listBox == SundayTimeSlots)
                return DayOfWeek.Sunday.ToString();
            else
                return "";
        }

        private void TrainerProfile_click(object sender, RoutedEventArgs e)
        {
            TrainerProfile trainerProfile = new TrainerProfile();
            this.NavigationService.Navigate(trainerProfile);
        }

        private void Messages_button(object sender, RoutedEventArgs e)
        {

        }

        private void Requests_button(object sender, RoutedEventArgs e)
        {

        }

        private void Calander_button(object sender, RoutedEventArgs e)
        {
            CalendarTrainer calendarTrainer = new CalendarTrainer();
            this.NavigationService.Navigate(calendarTrainer);
        }
    }
}
