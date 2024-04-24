using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Trainer : User
    {
        public string Location { get; set; }
        public WorkoutTypes Workout { get; set; }
        public double Price { get; set; }
        public string Education { get; set; }

        // Updated constructor to include the password parameter
        public Trainer(int userId, string name, string surname, string password, string emailAdress, string location, WorkoutTypes workout, double price, string education)
            : base(userId, name, surname, password, emailAdress) // Correctly pass all required parameters
        {
            Location = location;
            Workout = workout;
            Price = price;
            Education = education;
            UserRole = Role.Trainer; // Setting the UserRole to Trainer
        }
    }
}
