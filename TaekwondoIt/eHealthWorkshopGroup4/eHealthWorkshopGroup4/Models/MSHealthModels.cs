using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHealthWorkshopGroup4.Models
{
    public class HeartRateZones
    {
        public int underHealthyHeart { get; set; }
        public int underAerobic { get; set; }
        public int healthyHeart { get; set; }
    }

    public class PerformanceSummary
    {
        public int finishHeartRate { get; set; }
        public HeartRateZones heartRateZones { get; set; }
    }

    public class DistanceSummary
    {
        public string period { get; set; }
    }

    public class CaloriesBurnedSummary
    {
        public string period { get; set; }
        public int totalCalories { get; set; }
    }

    public class HeartRateSummary
    {
        public string period { get; set; }
        public int averageHeartRate { get; set; }
        public int peakHeartRate { get; set; }
        public int lowestHeartRate { get; set; }
    }

    public class GuidedWorkoutActivity
    {
        public string activityType { get; set; }
        public int roundsPerformed { get; set; }
        public int repetitionsPerformed { get; set; }
        public string exerciseTypeName { get; set; }
        public PerformanceSummary performanceSummary { get; set; }
        public DistanceSummary distanceSummary { get; set; }
        public int stepsTaken { get; set; }
        public string id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime dayId { get; set; }
        public string name { get; set; }
        public string duration { get; set; }
        public CaloriesBurnedSummary caloriesBurnedSummary { get; set; }
        public HeartRateSummary heartRateSummary { get; set; }
        public List<string> dataSourceTypes { get; set; }
    }

    public class MSHealthActivities
    {
        public List<GuidedWorkoutActivity> guidedWorkoutActivities { get; set; }
        public int itemCount { get; set; }
        public List<Exercise> ToExercises()
        {
            if (itemCount == 0)
            {
                return new List<Exercise>();
            }
            return new List<Exercise>(guidedWorkoutActivities.Select(Workout2Exercise).Where(x=>(x!=null)));
        }

        private Exercise Workout2Exercise(GuidedWorkoutActivity x)
        {
            if (x.heartRateSummary.peakHeartRate > 0)
            {
                return new Exercise(x.startTime, App.MyUserName, x.heartRateSummary.averageHeartRate,
                    x.heartRateSummary.peakHeartRate, (int)(x.endTime - x.startTime).TotalMinutes, x.name);
            }
            return null;
        }
    }
}
