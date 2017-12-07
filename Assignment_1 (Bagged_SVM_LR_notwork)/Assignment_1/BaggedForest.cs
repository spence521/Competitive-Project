using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class BaggedForest
    {
        public double Train_Accuracy { get; set; }
        public double Test_Accuracy { get; set; }
        public double Eval_Accuracy { get; set; }
        public List<Prediction> Train_Predictions { get; set; }
        public List<Prediction> Test_Predictions { get; set; }
        public List<Prediction> Eval_Predictions { get; set; }
        public BaggedForest(double train_accuracy, List<Prediction> train_predictions, double test_accuracy, List<Prediction> test_predictions,
            double eval_accuracy, List<Prediction> eval_predictions)
        {
            Train_Accuracy = train_accuracy;
            Test_Accuracy = test_accuracy;
            Eval_Accuracy = eval_accuracy;
            Train_Predictions = train_predictions;
            Test_Predictions = test_predictions;
            Eval_Predictions = eval_predictions;
        }   
    }
}
