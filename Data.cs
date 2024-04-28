
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

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
                "database=fitfinder1"; // Update the database name here
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

        public void InsertUser(User user)
        {
            OpenConnection();

            string query = "INSERT INTO Users (Name, Surname, Email, Password, ProfilePic, GenderId) VALUES (@Name, @Surname, @Email, @Password, @ProfilePic, @GenderId)";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Use the data from the User object to set the query parameters
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Surname", user.Surname);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@ProfilePic", user.ProfilePic); // Assuming it's a byte array
            cmd.Parameters.AddWithValue("@GenderId", user.GenderId);

            try
            {
                cmd.ExecuteNonQuery(); // Execute the SQL command to insert the new user
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message); // Log any exceptions
            }
            finally
            {
                CloseConnection(); // Ensure the connection is closed
            }
        }

    }
}


    


