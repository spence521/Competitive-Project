﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class InformationGain
    {
        public double ScreenNameLength { get; set; }
        public double DescriptionLength { get; set; }
        public double Days { get; set; }
        public double Hours { get; set; }
        public double Minutes { get; set; }
        public double Seconds { get; set; }
        public double Following { get; set; }
        public double Followers { get; set; }
        public double Ratio { get; set; }
        public double TotalTweets { get; set; }
        public double TweetsPerDay { get; set; }
        public double AverageLinks { get; set; }
        public double AverageUniqueLinks { get; set; }
        public double AverageUsername { get; set; }
        public double AverageUniqueUsername { get; set; }
        public double ChangeRate { get; set; }


        public InformationGain(double screenNameLen, double desLength, double days, double hours, double minutes, double seconds,
            double following, double followers, double ratio1, double totalTweets, double tweetsPerDay, double aveLinks, double aveUniqueLinks, double aveUsername,
            double aveUniqueUsername, double changeRt)
        {
            ScreenNameLength = screenNameLen;
            DescriptionLength = desLength;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Following = following;
            Followers = followers;
            Ratio = ratio1;
            TotalTweets = totalTweets;
            TweetsPerDay = tweetsPerDay;
            AverageLinks = aveLinks;
            AverageUniqueLinks = aveUniqueLinks;
            AverageUsername = aveUsername;
            AverageUniqueUsername = aveUniqueUsername;
            ChangeRate = changeRt;
        }
        public InformationGain()
        {
            ScreenNameLength = 0;
            DescriptionLength = 0;
            Days = 0;
            Hours = 0;
            Minutes = 0;
            Seconds = 0;
            Following = 0;
            Followers = 0;
            Ratio = 0;
            TotalTweets = 0;
            TweetsPerDay = 0;
            AverageLinks = 0;
            AverageUniqueLinks = 0;
            AverageUsername = 0;
            AverageUniqueUsername = 0;
            ChangeRate = 0;
        }
        public List<double> ToList()
        {
            return new List<double>
            {
                ScreenNameLength, DescriptionLength, Days, Hours, Minutes, Seconds, Following, Followers, Ratio, TotalTweets, TweetsPerDay, AverageLinks,
                AverageUniqueLinks, AverageUsername, AverageUniqueUsername, ChangeRate
            };
        }

    }
}
