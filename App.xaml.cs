using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Fitfinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           /* try
            {
                Data data = new Data();

                // Create a sample User
                User newUser = new User(
                   name: "John",
                   surname: "Doe",
                   email: "john.doe@example.com",
                   password: "password123",
                   profilePic: null,
                   genderId: 1
               );

                // Insert the new user into the database
                int insertedId = data.InsertUser(newUser);

                // Check if insertion was successful
                if (insertedId > 0)
                {
                    // Display a message box for confirmation
                    MessageBox.Show($"User inserted successfully with ID: {insertedId}");
                }
                else
                {
                    // Indicate failure
                    MessageBox.Show("Failed to insert user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.Message}");
            }*/
        }
    }
}
