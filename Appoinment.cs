using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Appointment
    {
        public int TrainerID { get; set; }
        public int ClientID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }

        public Appointment(int trainerId, int clientId, DateTime date, TimeSpan duration, string location)
        {
            TrainerID = trainerId;
            ClientID = clientId;
            Date = date;
            Duration = duration;
            Location = location;
        }

        // Method to get the end time of the appointment based on duration
        public DateTime EndTime()
        {
            return Date + Duration;
        }

        // Method to check if a user is authorized to view the appointment
        public bool IsUserAuthorized(int userId)
        {
            return userId == TrainerID || userId == ClientID;
        }

        public override string ToString()
        {
            return $"Appointment with Trainer {TrainerID} and Client {ClientID} at {Location} on {Date}. Duration: {Duration}.";
        }
    }
}
