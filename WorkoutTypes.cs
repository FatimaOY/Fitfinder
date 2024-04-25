using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    /*public enum WorkoutTypes
    {
        Weightlifting,
        Cardio,
        Stretching,
        Yoga,
        Pilates,
        CrossFit,
        Calinistics,
        Swimming
    }*/
    public class WorkoutType
    {
        public int WorkoutTypeId { get; set; }
        public string Name { get; set; }

        public List<TrainerWorkout> TrainerWorkouts { get; set; }

        // Constructor
        public WorkoutType(int workoutTypeId, string name)
        {
            WorkoutTypeId = workoutTypeId;
            Name = name;
            TrainerWorkouts = new List<TrainerWorkout>();
        }
    }
}
