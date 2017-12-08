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
        public List<Prediction> Train_Predictions { get; set; }
        public List<Prediction> Test_Predictions { get; set; }

        public List<Prediction> Eval_Predictions { get; set; }

        public BaggedForest(double accuracy, List<Prediction> train_predictions, List<Prediction> test_predictions, List<Prediction> eval_predictions)
        {
            Accuracy = accuracy;
            Train_Predictions = train_predictions;
            Test_Predictions = test_predictions;
            Eval_Predictions = eval_predictions;
        }
    }
}
