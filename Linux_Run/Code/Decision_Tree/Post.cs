using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Post
    {
        public int ID { get; set; }
        public int post { get; set; }
        public Post(int iD, int p)
        {
            ID = iD;
            post = p;
        }
    }
}
