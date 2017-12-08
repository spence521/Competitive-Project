using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class BaggedForest
    {
        public double Accuracy { get; set; }
        public List<Prediction> Predictions { get; set; }
        public BaggedForest(double accuracy, List<Prediction> predictions)
        {
            Accuracy = accuracy;
            Predictions = predictions;
        }   
    }
}
