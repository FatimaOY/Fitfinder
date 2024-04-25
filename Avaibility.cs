using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Availability
    {
        public int AvailabilityId { get; set; }
        public int TrainerId { get; set; }
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public PersonalTrainer PersonalTrainer { get; set; }

        public Availability(
            int availabilityId,
            int trainerId,
            string day,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            AvailabilityId = availabilityId;
            TrainerId = trainerId;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
