using System;

namespace YelpEngine
{
    public class UserClass
    {
        // ##### properties #####

        /// <summary>
        /// the total amount of tips that this user has that have been marked as useful.
        /// </summary>
        private readonly int usefulTipCount;

        /// <summary>
        /// the total amount of tips that this user has that have been marked as funny.
        /// </summary>
        private readonly int funnyTipCount;

        /// <summary>
        /// the total amount of tips that this user has that have been marked as cool.
        /// </summary>
        private readonly int coolTipCount;

        private string userId;
        private string name;
        private int average_stars;
        private int fans;
        private int tipCount;
        private int totalLikes;
        private DateTime yelping_since;

        private double userLat;
        private double userLon;

        public UserClass()
        {
        }

        // ##### Methods #####
        public UserClass(string name, string userId)
        {
            this.name = name;
            this.userId = userId;
        }

        public UserClass(string userId, string name, int average_stars, int fans, int coolTipCount, int tipCount, int totalLikes, int usefulTipCount, int funnyTipCount, DateTime yelping_since, double userLat, double userLon)
        {
            this.usefulTipCount = usefulTipCount;
            this.funnyTipCount = funnyTipCount;
            this.coolTipCount = coolTipCount;
            this.userId = userId;
            this.name = name;
            this.average_stars = average_stars;
            this.fans = fans;
            this.tipCount = tipCount;
            this.totalLikes = totalLikes;
            this.yelping_since = yelping_since;
            this.userLat = userLat;
            this.userLon = userLon;
        }
        public int Average_stars
        {
            get => average_stars;
        }

        public DateTime Yelping_since
        {
            get => yelping_since;
        }

        public int UsefulTipCount
        {
            get => usefulTipCount;
        }

        public int FunnyTipCount
        {
            get => funnyTipCount;
        }

        public int CoolTipCount
        {
            get => coolTipCount;
        }

        public string UserId
        {
            get => userId;
        }

        public string Name
        {
            get => name;
        }

        public int Fans
        {
            get => fans;
        }

        public int TipCount
        {
            get => tipCount;
        }

        public int TotalLikes
        {
            get => totalLikes;
        }

        public double UserLat
        {
            get => userLat;
            set => userLat = value;
        }

        public double UserLon
        {
            get => userLon;
            set => userLon = value;
        }
    }
}