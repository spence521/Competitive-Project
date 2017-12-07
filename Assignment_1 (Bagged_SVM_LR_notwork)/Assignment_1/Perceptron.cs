using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Perceptron
    {
        public List<Entry> Training_Data { get; private set; }
        public List<Entry> Test_Data { get; private set; }
        public List<int> Labels { get; set;}
        public double Learning_Rate { get; set; }
        public double Initial_Learning_Rate { get; set; }
        public bool DymanicLearningRate { get; set; }
        public int T_Count { get; set; }
        public double Margin { get; set; }
        public WeightBias WeightBias_Average { get; set; }
        public bool Aggressive { get; set; }
        public double C { get; set; }
        public bool SVM { get; set; }
        public double Tradeoff { get; set; }
        public bool Logistic_Regression { get; set; }
        public int ForestSize { get; set; }
        public Perceptron(List<Entry> train, List<Entry> test, double learning_rate, bool dymanicLearningRate, double margin, WeightBias wb_average, bool aggressive,
            double c, bool svm, double tradeoff, bool logistic_regression, int forestSize)
        {
            Training_Data = train;
            Test_Data = test;
            Learning_Rate = learning_rate;
            Initial_Learning_Rate = learning_rate;
            DymanicLearningRate = dymanicLearningRate;
            if (DymanicLearningRate) { T_Count = 1; }
            Margin = margin;   
            if(wb_average != null) { WeightBias_Average = wb_average; }
            Aggressive = aggressive;
            Labels = new List<int>();
            C = c;
            SVM = svm;
            Tradeoff = tradeoff;
            Logistic_Regression = logistic_regression;
            ForestSize = forestSize;
        }

        public WeightBias CalculateWB(WeightBias wb)
        {
            double[] w = wb.Weight;
            double b = wb.Bias;
            int updates = wb.Updates;
            foreach (var item in Training_Data)
            {
                if (DymanicLearningRate) { Learning_Rate = Initial_Learning_Rate / T_Count; }
                int y = item.Sign;
                int yguess;
                double[] x = item.Vector;
                double xw = 0;
                for (int i = 0; i < ForestSize; i++)
                {
                    xw = xw + (x[i] * w[i]);
                }
                xw += b;
                if (Logistic_Regression) //Logistic Regression
                {
                    for(int i = 0; i < ForestSize; i++) //(var xi in x)  //foreach (KeyValuePair<int, double> wi in w) //update this ForestSize here.
                    {
                        if (x[i] != 0)
                        {
                            w[i] = ((1 - (2 * Learning_Rate / Tradeoff)) * w[i]) + ((Learning_Rate * y * x[i]) / (Math.Exp(y * xw) + 1));
                        }
                    }
                    b = ((1 - (2 * Learning_Rate / Tradeoff)) * b) + ((Learning_Rate * y) / (Math.Exp(y * b) + 1));

                    updates++;
                }
                else if (SVM)
                {
                    if (y * xw <= 1)
                    {
                        for (int i = 0; i < ForestSize; i++) //foreach (KeyValuePair<int, double> wi in w) //update this ForestSize here.
                        {
                            if (x[i] != 0)
                            {
                                w[i] = ((1 - Learning_Rate) * w[i]) + (Learning_Rate * C * y * x[i]);
                            }
                        }
                        b = ((1 - Learning_Rate) * b) + (Learning_Rate * C * y);
                        updates++;
                    }
                    else
                    {
                        for (int i = 0; i < ForestSize; i++) //foreach (var xi in x)
                        {
                            if (x[i] != 0)
                            {
                                w[i] = ((1 - Learning_Rate) * w[i]);
                            }
                        }
                        b = ((1 - Learning_Rate) * b);
                    }
                }
                else //Perceptron
                {
                    if (xw >= Margin) { yguess = +1; }
                    else { yguess = -1; }
                    if (y != yguess)
                    {
                        if (Aggressive)
                        {
                            double rhs = y * xw;
                            double top = Margin - rhs;
                            double xx = 0;
                            for (int i = 0; i < ForestSize; i++)
                            {
                                xx = xx + (x[i] * x[i]);
                            }
                            xx++;
                            Learning_Rate = top / xx;
                            for (int i = 0; i < ForestSize; i++)
                            {
                                w[i] = w[i] + (Learning_Rate * y * x[i]);
                            }
                            b = b + (Learning_Rate * y);
                        }
                        else
                        {
                            for (int i = 0; i < ForestSize; i++)
                            {
                                w[i] = w[i] + (Learning_Rate * y * x[i]);
                            }
                            b = b + (Learning_Rate * y);
                        }
                        updates++;
                    }
                    if (DymanicLearningRate) { T_Count++; }
                    if (WeightBias_Average != null)
                    {
                        for (int i = 0; i < ForestSize; i++)
                        {
                            WeightBias_Average.Weight[i] += w[i];
                        }
                        WeightBias_Average.Bias += b;
                    }
                }
            }
            return new WeightBias(w, b, updates);
        }

        public double GetAccuracy(List<Entry> test_Data, WeightBias wb)
        {
            double[] w = wb.Weight;
            double b = wb.Bias;
            double TotalErrors = 0;
            foreach (var item in test_Data)
            {
                int y = item.Sign;
                int yguess;
                double[] x = item.Vector;
                double xw = 0;
                for (int i = 0; i < ForestSize; i++)
                {
                    xw = xw + (x[i] * w[i]);
                }
                xw += b;
                if(xw >= 0)
                {
                    yguess = +1;
                    Labels.Add(yguess);
                }
                else
                {
                    yguess = -1;
                    Labels.Add(0);
                }
                if (y != yguess)
                {
                    TotalErrors++;
                }
            }
            return 100 - ((TotalErrors / Convert.ToDouble(test_Data.Count)) * 100);
        }
        public void ShuffleTraining_Data(Random rSeed)
        {
            Training_Data = Training_Data.OrderBy(i => rSeed.Next()).ToList();
        }
    }
}
