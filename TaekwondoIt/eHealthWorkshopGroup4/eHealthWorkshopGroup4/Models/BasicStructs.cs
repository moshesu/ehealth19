using System;

namespace eHealthWorkshopGroup4.Models
{

    // Represents a user's rank e.i his or her belt color
    public enum Rank
    {
        Beginner,
        White,
        Yellow,
        Blue,
        Red,
        Black1,
        Black2,
        Black3,
        Black4,
        Black5,
        Black6,
        Black7,
        Black8
    }

    public class Poomsae
    {
        public static readonly Poomsae P1 = new Poomsae("IL", Rank.White, 20);
        public static readonly Poomsae P2 = new Poomsae("YI", Rank.Yellow, 20);
        public static readonly Poomsae P3 = new Poomsae("SAM", Rank.Yellow, 25);
        public static readonly Poomsae P4 = new Poomsae("SA", Rank.Blue, 30);
        public static readonly Poomsae P5 = new Poomsae("OH", Rank.Blue, 35);
        public static readonly Poomsae P6 = new Poomsae("YUK", Rank.Blue, 40);
        public static readonly Poomsae P7 = new Poomsae("CHIL", Rank.Red, 40);
        public static readonly Poomsae P8 = new Poomsae("PAL", Rank.Red, 45);

        public enum Taegeuk
        {
            IL, YI, SAM, SA, OH, YUK, CHIL, PAL
        }

        public Poomsae(string name, Rank minRank, int bonus)
        {
            Enum.TryParse(name, out Taegeuk tmp);
            this.name = tmp;
            this.minRank = minRank;
            xpBonus = bonus;
        }

        public static System.Collections.Generic.IEnumerable<Poomsae> Values
        {
            get
            {
                yield return P1;
                yield return P2;
                yield return P3;
                yield return P4;
                yield return P5;
                yield return P6;
                yield return P7;
                yield return P8;
            }
        }

        public Taegeuk name { get; private set; }
        public string Name { get { return name.ToString(); } }
        public Rank minRank { get; private set; }
        public int xpBonus { get; private set; }

        public override string ToString() => string.Format("TAEGEUK {0} JANG", name);

        private static bool myRandBool(int a) => new Random().Next(3) > a;
        public static Poomsae GetPoomsaeForRank(Rank r)
        {
            switch (r)
            {
                case Rank.Beginner:
                case Rank.White: return P1;
                case Rank.Yellow: return myRandBool(0) ? P3 : P2;
                case Rank.Blue: return myRandBool(0) ? (myRandBool(1) ? P6 : P5) : P4;
                case Rank.Red: return myRandBool(0) ? P8 : P7;
                default: return P8;
            }
        }
    }

    public class Message
    {
        public Message(string title, string groupName, string content, DateTime date)
        {
            Title = title;
            GroupName = groupName;
            Content = content;
            Date = date;
        }

        public string Title { get; set; }
        public string GroupName { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

    }

    public class Exercise
    {
        public DateTime startTime { get; set; }
        public string uname { get; set; }
        public double avgHR { get; set; }
        public double peakHR { get; set; }
        public int duration { get; set; }
        public string exerciseName { get; set; }

        public Exercise(DateTime startTime, string uname, double avgHR, double peakHR, int duration, string exerciseName)
        {
            this.startTime = startTime;
            this.uname = uname;
            this.avgHR = avgHR;
            this.peakHR = peakHR;
            this.duration = duration;
            this.exerciseName = exerciseName;
        }
    }

    /** Has no coachName field because this is determined by the training group */
    public class UserProfileData
    {
        public const string DEFAULT_COACH = "ip man";
        public const string DEFAULT_GROUP = "no group";


        /** @precond no input string contains tha '-' character */
        public UserProfileData(string fullname, string uname, Rank r, bool isCoach, string group)
        {
            this.fullname = fullname;
            this.uname = uname;
            rank = r;
            this.isCoach = isCoach;
            this.group = group;
        }

        internal UserProfileData(string fullname, string uname, Rank r, bool isCoach, string group, int XP, DateTime LTD) : this(fullname, uname, r, isCoach, group)
        {
            lastTrainingDate = LTD;
            xp = XP;
        }

        public string fullname { get; set; }
        public string uname { get; set; }
        public Rank rank { get; set; }

        // isCoach = true iff the user is a coach 
        public bool isCoach { get; set; }
        public string group { get; set; }
        public int xp { get; set; }

        public DateTime lastTrainingDate { get; set; }
    }

}