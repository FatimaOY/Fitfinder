
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data.SqlClient;

namespace Fitfinder
{
    public class Data
    {
        // Connection string to connect to MySQL
        /*private string connectionString = "datasource=127.0.0.1; port=3306; username=root; password=; database=fitfinder1;";

        // Utility function to execute an INSERT query
        private int Insert(string query)
        {
            // Create a new MySqlConnection with the given connection string
            MySqlConnection connection = new MySqlConnection(connectionString);
            // Create a command to execute the SQL query
            MySqlCommand commandDatabase = new MySqlCommand(query, connection);
            try
            {
                // Open the database connection
                connection.Open();
                // Execute the query
                int result = commandDatabase.ExecuteNonQuery();
                // Get the last inserted ID
                int lastInsertedId = (int)commandDatabase.LastInsertedId;

                // Return the last inserted ID
                return lastInsertedId;
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Error inserting into database: {ex.Message}");
            }
            finally
            {
                // Always close the connection
                connection.Close();
            }

            // Return -1 in case of error
            return -1;
        }
        public int InsertUser(User user)
        {
            // Construct the INSERT SQL query
            string query = $"INSERT INTO Users(user_id, name, surname, email, password, gender_id) " +
                           $"VALUES (NULL, '{user.Name}', '{user.Surname}', '{user.Email}, '{user.Password}', {user.GenderId});";

            // Execute the INSERT query
            return this.Insert(query);
        }*/

        private string connectionString;
        private MySqlConnection connection;

        public Data()
        {
            connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4"; // Update the database name here
            connection = new MySqlConnection(connectionString);

        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public bool ValidateUserLogin(string email, string password)
        {
            // Logic to validate user login
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; // True if user is found
                }
            }
        }
        public UserInfo GetUserByEmail(string email)
        {
            // Fetch user information by email
            string query = "SELECT * FROM Users WHERE Email = @Email";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserInfo
                            {

                                Name = reader["Name"]?.ToString(),
                                userId = (int)reader["UserID"],
                                Surname = reader["Surname"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                            };
                        }
                    }
                }
            }

            return null; // Return null if no user is found
        }
        public string GetUserRole(int userId)
        {
            // Check if user is a client, trainer, or admin
            if (CheckUserInTable("Client", userId))
            {
                return "Client";
            }
            if (CheckUserInTable("Trainer", userId))
            {
                return "Trainer";
            }
            if (CheckUserInTable("Admin", userId))
            {
                return "Admin";
            }

            return "Unknown"; // Default role if not found in any table
        }

        private bool CheckUserInTable(string tableName, int userId)
        {
            // Check if a user ID is present in a specific table
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE UserID = @UserID";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; // True if the user is found in the specified table
                }
            }
        }

        public int InsertUser(User user)
        {
            OpenConnection();

            string query = "INSERT INTO User (Name, Surname, Email, Password, ProfilePic, GenderId) VALUES (@Name, @Surname, @Email, @Password, @ProfilePic, @GenderId)";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Use the data from the User object to set the query parameters
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Surname", user.Surname);
            cmd.Parameters.AddWithValue("Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@ProfilePic", user.ProfilePic); // Assuming it's a byte array
            cmd.Parameters.AddWithValue("@GenderId", user.GenderId);

            int rowsAffected = cmd.ExecuteNonQuery(); // Execute command and get affected rows

            if (rowsAffected > 0)
            {
                int userId = (int)cmd.LastInsertedId; // Cast to int if necessary
                return userId; // Return the UserId
            }

            return -1; // Indicate failure to insert
        }

        //INSERTING A CLIENT

        public void InsertClient(int userId, Client client)
        {
            OpenConnection();

            // Assuming UserId is a foreign key in the Clients table
            string query = "INSERT INTO Client (PersonId, Description) VALUES (@PersonId, @Description)";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Use the data from the Client object to set the query parameters
            cmd.Parameters.AddWithValue("@PersonId", userId); // Use the UserId from InsertUser
            cmd.Parameters.AddWithValue("@Description", client.Description);


            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Client registered successfully.");
            }
            else
            {
                MessageBox.Show("Failed to register client.");
            }
        }

        //INSERTING A TRAINER
        public void InsertTrainer(int userId, Trainer trainer)
        {
            OpenConnection();

            // Assuming UserId is a foreign key in the Trainers table
            string query = "INSERT INTO Trainer (PersonId, Description, Location, Price) VALUES (@PersonId, @Description, @Location, @Price)";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Use the data from the Trainer object to set the query parameters
            cmd.Parameters.AddWithValue("@PersonId", userId); // Use the UserId from InsertUser
            cmd.Parameters.AddWithValue("@Description", trainer.Description);
            cmd.Parameters.AddWithValue("@Location", trainer.Location);
            cmd.Parameters.AddWithValue("@Price", trainer.Price);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Trainer registered successfully.");
            }
            else
            {
                MessageBox.Show("Failed to register trainer.");
            }
        }

        
        public UserInfo GetUserInformation(string email, string password)
        {
            UserInfo userInfo = null;
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            string query = "SELECT * FROM User WHERE email = @Email AND password = @Password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

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

        public int GetUserId(string email, string password)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int userId = -1; // Default value indicating user not found

            string query = "SELECT UserId FROM user WHERE Email = @Email AND Password = @Password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

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

        public int GetTrainerID(int userId)
        {
            string connectionString =
                "datasource=127.0.0.1;" +
                "port=3306;" +
                "username=root;" +
                "password=;" +
                "database=fitfinder4";

            int TrainerId = -1; // Default value indicating user not found

            string query = "SELECT TrainerId FROM Trainer WHERE PersonId = @PersonId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@PersonId", userId);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        TrainerId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return TrainerId;
        }
    }
}






