using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class ReTweets
    {
        public int ID { get; set; }
        public int ReTweet { get; set; }
        public ReTweets(int iD, int reTweet)
        {
            ID = iD;
            ReTweet = reTweet;
        }
    }
}
