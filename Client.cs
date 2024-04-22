using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Client: User
    {
        public string PrefferedLocation { get; set; }
        public Gender PrefferedGender { get; set; }
        public WorkoutTypes PrefferedWorkout { get; set; }
        public PriceRange PrefferedPrice { get; set; }

        public Client(int id, string name, string surname, string emailAdress, string prefferedLocation, Gender prefferedGender, WorkoutTypes prefferedWorkout, PriceRange prefferedPrice)
            : base(id, name, surname, emailAdress)
        {
            PrefferedLocation = prefferedLocation;
            PrefferedGender = prefferedGender;
            PrefferedWorkout = prefferedWorkout;
            PrefferedPrice = prefferedPrice;
        }

    }
    public struct PriceRange
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public PriceRange(decimal minPrice, decimal maxPrice)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}
