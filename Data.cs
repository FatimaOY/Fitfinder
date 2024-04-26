using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    /*public class Data
    {
        private string connectionString = "datasource=127.0.0.1; port=3306; username = root; password=; database = fitfinder1;";

        private const int _trainee = 1;
        private const int _personal_trainer = 2;
        private const int _admin = 3;

        //opening the connection
        private int Insert(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                int result = commandDatabase.ExecuteNonQuery();
                return (int)commandDatabase.LastInsertedId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }
        
    }

    /*public int InsertClient(User user)
    {
        string query = $"INSERT INTO person(ID,Name,Surname,password,profile_pic) " +
            $"VALUES (NULL, '{user.Name}','{user.Surname}',NULL,{user.EmailAdress});";

        return this.Insert(query);
    }
    }
    }*/
    public class Data
    {
        // Connection string to connect to MySQL
        private string connectionString = "datasource=127.0.0.1; port=3306; username=root; password=; database=fitfinder1;";

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
        }
    }
}
