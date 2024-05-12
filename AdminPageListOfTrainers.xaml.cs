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
    /// Interaction logic for AdminPageListOfTrainers.xaml
    /// </summary>
    public partial class AdminPageListOfTrainers : Page
    {
        public AdminPageListOfTrainers()
        {
            InitializeComponent();
            PopulateUserListView();
        }


        private void PopulateUserListView()
        {
            // Assuming you have a method to retrieve users from your database
            List<User> users = GetUsersFromDatabase();

            // Set the ListView's ItemsSource to the list of users
            userListView.ItemsSource = users;
        }

        // class representing a User
        public class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }

            public string Password { get; set; }
            public byte[] ProfilePic { get; set; }

            public int GenderId { get; set; }

            public string Gender { get; set; } // This will store the gender display value

            public string Description { get; set; }

            public string Location { get; set; }
            public int Price { get; set; }


            // Add more properties as needed
        }

        //method to retrieve users from the database
        private List<User> GetUsersFromDatabase()
        {
            List<User> users = new List<User>();

            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT u.name, u.surname, u.email, u.password,u.ProfilePic, u.GenderId, t.Description, t.Location, t.Price FROM user as u JOIN trainer as t ON u.userId = t.PersonId JOIN gender as g ON u.genderId = g.genderId;\r\n"; // Assuming your table name is 'user'
                // SELECT u.name, u.surname, u.email, u.password,u.ProfilePic ,g.name FROM user as u JOIN client as t ON u.userId = t.PersonId JOIN gender as g ON u.genderId = g.genderId;

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                GenderId = Convert.ToInt32(reader["GenderId"]),
                                Description = reader["description"].ToString(),
                                Location = reader["location"].ToString(),
                                Price = Convert.ToInt32(reader["price"])

                            };

                            // Set GenderId based on the value retrieved from the database
                            int genderId = user.GenderId; // Get the numeric gender ID
                            if (genderId == 1)
                            {
                                user.Gender = "Male";
                            }
                            else if (genderId == 2)
                            {
                                user.Gender = "Female";
                            }
                            else if (genderId == 3)
                            {
                                user.Gender = "Other";
                            }

                            //to display picture (for now its not working)
                            if (reader["ProfilePic"] != DBNull.Value)
                            {
                                user.ProfilePic = (byte[])reader["ProfilePic"];
                            }
                            else
                            {
                                // Handle the case where ProfilePic is NULL (e.g., set a default image)
                                // For now, I'm setting it to an empty byte array
                                user.ProfilePic = new byte[0];
                            }
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
        private void ProfileAdmin_button(object sender, RoutedEventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile();
            this.NavigationService.Navigate(adminProfile);
        }

        private void TraineesList_button(object sender, RoutedEventArgs e)
        {
            AdminMainPage adminMainPageListForTrainee = new AdminMainPage();
            this.NavigationService.Navigate(adminMainPageListForTrainee);
        }

        private void TrainersList_button(object sender, RoutedEventArgs e)
        {
            AdminPageListOfTrainers adminMainPageListForTrainer = new AdminPageListOfTrainers();
            this.NavigationService.Navigate(adminMainPageListForTrainer);
        }
    }
}
