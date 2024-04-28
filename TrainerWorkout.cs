using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class TrainerWorkout
    {
        public int TrainerWorkoutId { get; set; }
        public int WorkoutTypeId { get; set; }
        public int TrainerId { get; set; }

        public WorkoutType WorkoutType { get; set; }
        public Trainer PersonalTrainer { get; set; }

        public TrainerWorkout(int trainerWorkoutId, int workoutTypeId, int trainerId)
        {
            TrainerWorkoutId = trainerWorkoutId;
            WorkoutTypeId = workoutTypeId;
            TrainerId = trainerId;
        }
    }
}
