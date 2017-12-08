using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class TrainingData
    {
        public enum ScreenNameLength { Range1, Range2, Range3, Range4, Range5 }
        public enum DescriptionLength { Range1, Range2, Range3, Range4, Range5 }
        public enum LongevityDays { Range1, Range2, Range3, Range4, Range5 }
        public enum LongevityHours { Range1, Range2, Range3 }
        public enum LongevityMinSec { Range0_15, Range16_30, Range31_45, RangeGT_45 }
        public enum Follow { Range0_100, Range101_400, Range401_1400, Range1401_3000, Range3001_10000, RangeGT_10000 }
        public enum Ratio { Range0_1, Range1_3, Range3_8, RangeGT_8 }
        public enum Tweets { Range0_66, Range67_132, RangeGT_132 }
        public enum TweetsPerDay { Range0_66, Range67_132, RangeGT_132 }
        public enum AverageLinks { Range0_1, Range1_2, RangeGT_2 }
        public enum AverageUsername { Range0_2, Range2_4, RangeGT_4 }
        public enum ChangeRate { Range0_5, Range5_50, RangeGT_50 }
        public enum ReTweets { Range1, Range2, Range3, Range4, Range5, Range6 }
        //public enum Tags { Range1, Range2, Range3, Range4, Range5, Range6 }

        /// <summary>
        /// Features
        /// </summary>
        public ScreenNameLength screenNameLength { get; set; }
        public DescriptionLength descriptionLength { get; set; }
        public LongevityDays Days { get; set; }
        public LongevityHours Hours { get; set; }

        public LongevityMinSec Minutes { get; set; }
        public LongevityMinSec Seconds { get; set; }
        public Follow Following { get; set; }
        public Follow Followers { get; set; }

        public Ratio ratio { get; set; }
        public Tweets TotalTweets { get; set; }
        public TweetsPerDay tweetsPerDay { get; set; }
        public AverageLinks averageLinks { get; set; }

        public AverageLinks AverageUniqueLinks { get; set; }
        public AverageUsername averageUsername { get; set; }
        public AverageUsername AverageUniqueUsername { get; set; }
        public ChangeRate changeRate { get; set; }
        public ReTweets ReTweet { get; set; }
        //public Tags Tag { get; set; }
        public int Label { get; set; }

        public TrainingData(ScreenNameLength screenNameLen, DescriptionLength desLength, LongevityDays days, LongevityHours hours, LongevityMinSec minutes, LongevityMinSec seconds,
            Follow following, Follow followers, Ratio ratio1, Tweets totalTweets, TweetsPerDay tweetsPDay, AverageLinks aveLinks, AverageLinks aveUniqueLinks, AverageUsername aveUsername,
            AverageUsername aveUniqueUsername, ChangeRate changeRt, ReTweets reTweet, /*Tags tag,*/ int label)
        {
            screenNameLength = screenNameLen;
            descriptionLength = desLength;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Following = following;
            Followers = followers;
            ratio = ratio1;
            TotalTweets = totalTweets;
            tweetsPerDay = tweetsPDay;
            averageLinks = aveLinks;
            AverageUniqueLinks = aveUniqueLinks;
            averageUsername = aveUsername;
            AverageUniqueUsername = aveUniqueUsername;
            changeRate = changeRt;
            ReTweet = reTweet;
            //Tag = tag;
            Label = label;
        }
        public override string ToString()
        {
            return            
                screenNameLength.ToString() + " " +
                descriptionLength.ToString() + " " +
                Days.ToString() + " " +
                Hours.ToString() + " " +
                Minutes.ToString() + " " +
                Seconds.ToString() + " " +
                Following.ToString() + " " +
                Followers.ToString() + " " +
                ratio.ToString() + " " +
                TotalTweets.ToString() + " " +
                tweetsPerDay.ToString() + " " +
                averageLinks.ToString() + " " +
                AverageUniqueLinks.ToString() + " " +
                averageUsername.ToString() + " " +
                AverageUniqueUsername.ToString() + " " +
                changeRate.ToString() + " " +
                ReTweet.ToString() + " " +
                //Tag.ToString() + " " +
                Label;
        }


    }
}
