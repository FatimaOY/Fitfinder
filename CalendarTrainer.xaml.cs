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
            if (listBox != null && listBox.SelectedItem != null)
            {
                var timeSlot = listBox.SelectedItem.ToString();
                listBox.Foreground = Brushes.Blue; // Change foreground color to blue
                SaveAvailability(listBox); // Save availability to the database
            }
        }

        private void SaveAvailability(ListBox listBox)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    var currentUser = UserSession.CurrentUser;
                    Data data = new Data(); // Create an instance of the Data class
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO availability (TrainerId, Day) VALUES (@TrainerId, @Day)";
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetUserId((currentUser.Email).ToString(), (currentUser.Password).ToString())); // Change 0 to the actual TrainerId
                    cmd.Parameters.AddWithValue("@Day", GetDayOfWeek(listBox));

                    string[] times = listBox.SelectedItem.ToString().Split('-');
                    string startTimeString = times[0].Trim().Replace(" AM", "").Replace(" PM", ""); // Trim and remove " AM" and " PM"
                    string endTimeString = times[1].Trim().Replace(" AM", "").Replace(" PM", ""); // Trim and remove " AM" and " PM"

                    // Convert time strings to TimeSpan
                    TimeSpan startTime, endTime;
                    if (TimeSpan.TryParse(startTimeString, out startTime) && TimeSpan.TryParse(endTimeString, out endTime))
                    {
                        cmd.Parameters.AddWithValue("@StartTime", startTime);
                        cmd.Parameters.AddWithValue("@EndTime", endTime);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Invalid time format.");
                    }
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
                    cmd.Parameters.AddWithValue("@TrainerId", data.GetUserId((currentUser.Email).ToString(), (currentUser.Password).ToString())); // Change 0 to the actual TrainerId
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availabilities.Add(new Availability(
                                availabilityId: Convert.ToInt32(reader["AvailabilityId"]),
                                trainerId: Convert.ToInt32(reader["TrainerId"]),
                                day: reader["Day"].ToString(),
                                startTime: TimeSpan.Parse(reader["StartTime"].ToString()),
                                endTime: TimeSpan.Parse(reader["EndTime"].ToString())));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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
    }
}
