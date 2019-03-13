using System.Collections.Generic;
using System;
using System.Linq;

namespace SleepItOff
{

    public class ActivityResponse
    {
        public List<SleepActivity> sleepActivities { get; set; }
    }


    public class SleepSegment
    {
        public string sleepType { get; set; }   //'Unknown', 'UndifferentiatedSleep', 'RestlessSleep', 'RestfulSleep'
        public DateTime startTime { get; set; } 
        public DateTime endTime { get; set; } 
        public string segmentType { get; set; }  //'Doze', 'Sleep', 'Snooze', 'Awake'      
        public string duration { get; set; }


    }

    public class SleepActivity
    {
        public List<SleepSegment> activitySegments { get; set; }
        public DateTime fallAsleepTime { get; set; }
        public int sleepEfficiencyPercentage { get; set; }
        public int numberOfWakeups { get; set; }
        public string duration { get; set; }
        public DateTime dayId { get; set; }


    }

    public class ProfileResponse
    {
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public  string gender { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public DateTime birthdate { get; set; }

    }

    public class SummaryResponse
    {
        public List<Summary> summaries { get; set; }
    }

    public class Summary
    {
        public string userId { get; set; }
    }

    public class SegmentSummaryTable
    {
        public static SegmentSummary Awake { get; set; }
        public static SegmentSummary Snooze { get; set; }
        public static SegmentSummary Doze { get; set; }
        public static SegmentSummary RestlessSleep { get; set; }
        public static SegmentSummary RestfulSleep { get; set; }
        public static SegmentSummary REMSleep { get; set; }
        public static DateTime lastUpdated { get; set; }
        public static string userID { get; set; }

        //the order of the list is like the order of the fields in the object
        public static void updateTableByList(List<int> weightedSleepSegmentsStats)
        {
            if (weightedSleepSegmentsStats != null)
            {
                if (weightedSleepSegmentsStats.Count == (8 * 6))
                {
                    Awake.updateSemgentByList(weightedSleepSegmentsStats, 0);
                    Snooze.updateSemgentByList(weightedSleepSegmentsStats, 8*1);
                    Doze.updateSemgentByList(weightedSleepSegmentsStats, 8*2);
                    RestlessSleep.updateSemgentByList(weightedSleepSegmentsStats, 8*3);
                    RestfulSleep.updateSemgentByList(weightedSleepSegmentsStats, 8*4);
                    REMSleep.updateSemgentByList(weightedSleepSegmentsStats, 8*5);
                }
            }
        }

        public static void ClearSegmentTable()
        {
            Awake = null;
            Snooze = null;
            Doze = null;
            RestfulSleep = null;
            RestlessSleep = null;
            REMSleep = null;
            userID = "";
        }

        public static float getUserSeniority()
        {
            int sumOfSegmentsDuration = Awake.totalDuration + Snooze.totalDuration + Doze.totalDuration + RestlessSleep.totalDuration + RestfulSleep.totalDuration + REMSleep.totalDuration;
            if (sumOfSegmentsDuration < (60 * 8 * 3))//slept less than 3 days
            {
                return 0.25F;
            }
            else if (sumOfSegmentsDuration < (60 * 8 * 10))//slept less than 10 days
            {
                return 0.5F;
            }
            else if(sumOfSegmentsDuration < (60 * 8 * 30))//slept less than a month
            {
                return 0.75F;
            }
            else//slept more than a month
            {
                return 0.95F;
            }
        }
    }

    public class SegmentSummary
    {
        public int countTimes { get; set; }
        public int totalDuration { get; set; }
        public int timesToAwake { get; set; }
        public int timesToSnooze { get; set; }
        public int timesToDoze { get; set; }
        public int timesToRestlessSleep { get; set; }
        public int timesToRestfulSleep { get; set; }
        public int timesToREM { get; set; }

        public void updateSemgentByList(List<int> weightedSleepSegmentsStats, int index)
        {
                countTimes = weightedSleepSegmentsStats[index];
                totalDuration = weightedSleepSegmentsStats[index+1];
                timesToAwake = weightedSleepSegmentsStats[index+2];
                timesToSnooze = weightedSleepSegmentsStats[index+3];
                timesToDoze = weightedSleepSegmentsStats[index+4];
                timesToRestlessSleep = weightedSleepSegmentsStats[index+5];
                timesToRestfulSleep = weightedSleepSegmentsStats[index+6];
                timesToREM = weightedSleepSegmentsStats[index+7];
        }

        public int avgDuration() {            
            if (this.countTimes == 0){
                return 0;
            }
            else{
                return this.totalDuration / this.countTimes;
            }
        }
        public int toAwakePer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToAwake / countTimes)*100;
            }
        }
        public int toSnozePer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToSnooze / countTimes)*100;
            }
        }
        public int toDozePer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToDoze / countTimes)*100;
            }
        }
        public int toRestlessSleepPer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToRestlessSleep / countTimes)*100;
            }
        }
        public int toRestfullSleepPer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToRestfulSleep / countTimes)*100;
            }
        }
        public int toREMPer()
        {
            if (this.countTimes == 0)
            {
                return 0;
            }
            else
            {
                return (timesToREM / countTimes) * 100;
            }
        }
        public int maxChanceToNextStage() //returns next stage with most probability. (int)SegmentStage
        {
            int[] perArray = { toAwakePer(), toSnozePer(), toDozePer(), toRestlessSleepPer(), toRestfullSleepPer(), toREMPer() };            
            int maxValue = perArray.Max();
            int maxIndex = perArray.ToList().IndexOf(maxValue);
            return maxIndex + 1;
        }


    }


}
