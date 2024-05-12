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
        public BrowseTrainers()
        {
            InitializeComponent();
            List<TrainerBrowse> trainers = GetTrainersFromDatabase();

            TrainersListBox.ItemsSource = trainers;
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
