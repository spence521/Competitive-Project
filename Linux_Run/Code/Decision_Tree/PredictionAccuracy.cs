﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class PredictionAccuracy
    {
        public List<Prediction> Predictions { get; set; }
        public double Accuracy { get; set; }
        public DecisionTree Tree { get; set; }
        public PredictionAccuracy(List<Prediction> predictions, double accuracy, DecisionTree tree)
        {
            Predictions = predictions;
            Accuracy = accuracy;
            Tree = tree;
        }
    }
}
