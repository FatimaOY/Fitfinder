using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    /*public class Trainer : User
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
    }*/

    public class Trainer : User
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int Experience { get; set; }
        public string Certifications { get; set; }
        public bool IsActive { get; set; }

        public List<TrainerWorkout> TrainerWorkouts { get; set; }
        public List<Availability> Availabilities { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Message> Messages { get; set; }

        public Trainer(
            int userId,
            string name,
            string surname,
            string email,
            string password,
            byte[] profilePic,
            int genderId,  // Added parameter
            string description,
            string location,
            decimal price,
            string favoriteColor, 
            string dreamDestination, 
            string favoriteAnimal


        ) : base(name, surname, email, password, profilePic, genderId, favoriteColor,dreamDestination,favoriteAnimal)  // Correct base constructor call
        {
            Description = description;
            Location = location;
            Price = price;


            TrainerWorkouts = new List<TrainerWorkout>();
            Availabilities = new List<Availability>();
            Appointments = new List<Appointment>();
            Messages = new List<Message>();
         
        }
    }
}
