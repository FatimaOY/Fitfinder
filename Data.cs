using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Data
    {
        private string connectionString = "datasource=127.0.0.1; port=3306; username = root; password=; database = fitfinder;";

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

    public int InsertClient(User user)
    {
        string query = $"INSERT INTO person(ID,Name,Surname,password,profile_pic) " +
            $"VALUES (NULL, '{user.Name}','{user.Surname}',NULL,{user.EmailAdress});";

        return this.Insert(query);
    }
}
