using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    /*public class Client: User
    {
        public string PrefferedLocation { get; set; }
        public Gender PrefferedGender { get; set; }
        public WorkoutTypes PrefferedWorkout { get; set; }
        public PriceRange PrefferedPrice { get; set; }

        public Client(int id, string name, string surname, string password, string emailAdress, string prefferedLocation, Gender prefferedGender, WorkoutTypes prefferedWorkout, PriceRange prefferedPrice)
            : base(id, name, surname,password, emailAdress)
        {
            PrefferedLocation = prefferedLocation;
            PrefferedGender = prefferedGender;
            PrefferedWorkout = prefferedWorkout;
            PrefferedPrice = prefferedPrice;
        }

    }*/
    public class Trainee : User
    {
        public string Description { get; set; }
        public string Goals { get; set; }

        public List<Message> Messages { get; set; }
        public List<Appointment> Appointments { get; set; }

        // Constructor for Trainee, calls the base constructor
        public Trainee(
            int userId,
            string name,
            string surname,
            string email,
            string password,
            byte[] profilePic,
            int genderId,
            string description,
            string goals) : base( name, surname,email, password, profilePic, genderId)
        {
            Description = description;
            Goals = goals;

            Messages = new List<Message>();
            Appointments = new List<Appointment>();
        }
    }

}
