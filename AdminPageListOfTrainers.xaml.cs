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

        private Data data;


        public static AdminPageListOfTrainers.User SelectedUser { get; set; }
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
            public List<string> WorkoutTypes { get; set; }  // Changed to List<string>


            // Add more properties as needed
        }

    //method to retrieve users from the database
    private List<User> GetUsersFromDatabase()
        {
            Data data = new Data(); // Create an instance of the Data class
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
                string queryForWokouts = "SELECT wt.Name FROM `trainerworkout` as tw JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId WHERE tw.PersonalTrainer = @PersonalTrainer;\r\n";
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
                                Price = Convert.ToInt32(reader["price"]),
                                WorkoutTypes = new List<string>()


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

                // Now fetch workout types for each user
                foreach (var user in users)
                {
                    string queryForWorkouts = @"
                SELECT wt.Name 
                FROM trainerworkout AS tw 
                JOIN workouttypes AS wt ON tw.WorkoutType = wt.WorkoutTypeId 
                WHERE tw.PersonalTrainer = @PersonalTrainer";

                    using (MySqlCommand command = new MySqlCommand(queryForWorkouts, connection))
                    {
                        int userId = data.GetUserId(user.Email, user.Password);
                        int trainerID = data.GetTrainerID(userId);
                        command.Parameters.AddWithValue("@PersonalTrainer", trainerID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.WorkoutTypes.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }

            return users;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            User user = button.DataContext as User;

            SelectedUser = user;
            AdminEditTrainer adminEditProfile = new AdminEditTrainer(SelectedUser);
            this.NavigationService.Navigate(adminEditProfile);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            Button button = sender as Button;

            // Get the corresponding user from the button's DataContext
            User user = button.DataContext as User;

            // Perform the deletion operation for the user
            DeleteUserFromDatabase(user);

            // Refresh the ListView after deletion
            PopulateUserListView();
        }

        private void DeleteUserFromDatabase(User user)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM user WHERE name = @Name AND surname = @Surname AND email = @Email";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Surname", user.Surname);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.");
                    }
                }
            }
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

        private void createProfile_button(object sender, RoutedEventArgs e)
        {
            AdminCreateTrainer adminMainPage = new AdminCreateTrainer();
            this.NavigationService.Navigate(adminMainPage);
        }
    }
}
