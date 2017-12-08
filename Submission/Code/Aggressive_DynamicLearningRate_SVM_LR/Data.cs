using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assignement_2
{
    public class Data
    {
        public List<Entry> Training_Data { get; private set; }
        public List<Entry> Test_Data { get; private set; }
        public List<Entry> Cross_Validate_Data { get; private set; }
        public List<Entry> Cross_1 { get; private set; }
        public List<Entry> Cross_2 { get; private set; }
        public List<Entry> Cross_3 { get; private set; }
        public List<Entry> Cross_4 { get; private set; }
        public List<Entry> Cross_5 { get; private set; }
        public List<Prediction> Predictions { get; private set; }
        public Dictionary<int, AccuracyWB> AccuracyWeightB { get; private set; }
        public Perceptron perceptron { get; set; }
        public double Accuracy { get; set; }
        public double Learning_Rate { get; set; }
        public double Margin { get; set; }
        public double Majority { get; set; }
        public WeightBias BestWeightBias { get; set; }

        public double C { get; set; }
        public bool SVM { get; set; }
        public double Tradeoff { get; set; }
        public bool Logistic_Regression { get; set; }

        public Data(int epochs, double learning_rate, Random r, bool DymanicLearningRate, double margin, bool Average, bool Aggressive, 
            double c, bool svm, double tradeoff, bool logistic_regression, StreamReader train, StreamReader test)
        {
            C = c;
            SVM = svm;
            Tradeoff = tradeoff;
            Logistic_Regression = logistic_regression;

            double[] w_average = new double[16];
            double b_average;
            WeightBias wb_average = null;
            if (Average)
            {
                for (int i = 0; i < 16; i++)
                {
                    double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                    w_average[i] = randomNumber;
                }
                b_average = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                wb_average = new WeightBias(w_average, b_average, 0);
            }

            double temp_accuracy1;
            double temp_accuracy2;
            double temp_accuracy3;
            double temp_accuracy4;
            double temp_accuracy5;
            Learning_Rate = learning_rate;
            Margin = margin;
            Cross_Validate_Data = new List<Entry>();
            Cross_1 = new List<Entry>();
            Cross_2 = new List<Entry>();
            Cross_3 = new List<Entry>();
            Cross_4 = new List<Entry>();
            Cross_5 = new List<Entry>();
            SetValidateData(train, test, r);

            #region First Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_4))).ToList();
            Test_Data = Cross_5;

            //SetData(r1, r5);
            //SetData(r2);
            //SetData(r3);
            //SetData(r4);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            double[] w = new double[16];
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            for (int i = 0; i < 16; i++)
            {
                double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                w[i] = randomNumber;
            }
            WeightBias wb = new WeightBias(w, b, 0);            
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy1 = perceptron.GetAccuracy(Test_Data, wb);
            if(Average) { temp_accuracy1 = perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average); }
            #endregion

            #region Second Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_5))).ToList();
            Test_Data = Cross_4;

            //SetData(r1, r4);
            //SetData(r2);
            //SetData(r3);
            //SetData(r5);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy2 = perceptron.GetAccuracy(Test_Data, wb);
            if (Average) { temp_accuracy2 = perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average); }
            #endregion

            #region Third Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_3;

            //SetData(r1, r3);
            //SetData(r2);
            //SetData(r4);
            //SetData(r5);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy3 = perceptron.GetAccuracy(Test_Data, wb);
            if (Average) { temp_accuracy3 = perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average); }
            #endregion

            #region Fourth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_2;

            //SetData(r1, r2);
            //SetData(r3);
            //SetData(r4);
            //SetData(r5);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy4 = perceptron.GetAccuracy(Test_Data, wb);
            if (Average) { temp_accuracy4 = perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average); }
            #endregion

            #region Fifth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_2.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_1;

            //SetData(r2, r1);
            //SetData(r3);
            //SetData(r4);
            //SetData(r5);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy5 = perceptron.GetAccuracy(Test_Data, wb);
            if (Average) { temp_accuracy5 = perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average); }
            #endregion
            
            Accuracy = (temp_accuracy1 + temp_accuracy2 + temp_accuracy3 + temp_accuracy4 + temp_accuracy5) / 5;
        }
        public Data(StreamReader r1, StreamReader r2, int epochs, double learning_rate, Random r, bool DymanicLearningRate, double margin, bool Average, bool Aggressive,
            double c, bool svm, double tradeoff, bool logistic_regression)
        {
            C = c;
            SVM = svm;
            Tradeoff = tradeoff;
            Logistic_Regression = logistic_regression;

            double[] w_average = new double[16];
            double b_average;
            WeightBias wb_average = null;
            if (Average)
            {
                for (int i = 0; i < 16; i++)
                {
                    double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                    w_average[i] = randomNumber;
                }
                b_average = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                wb_average = new WeightBias(w_average, b_average, 0);
            }
            
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            AccuracyWeightB = new Dictionary<int, AccuracyWB>();
            SetData(r1, r2);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, DymanicLearningRate, margin, wb_average, Aggressive, C, SVM, Tradeoff, Logistic_Regression);
            double[] w = new double[16];
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            for (int i = 0; i < 16; i++)
            {
                double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
                w[i] = randomNumber;
            }
            WeightBias wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                if (Average)
                {
                    perceptron.WeightBias_Average.Updates = wb.Updates;
                    AccuracyWeightB.Add(i+1, new AccuracyWB(perceptron.GetAccuracy(Test_Data, perceptron.WeightBias_Average), perceptron.WeightBias_Average));
                    
                }
                else
                {
                    AccuracyWeightB.Add(i + 1, new AccuracyWB (perceptron.GetAccuracy(Test_Data, wb), wb));
                }
                perceptron.ShuffleTraining_Data(r);
            }
            //foreach (var item in AccuracyWeightB)
            //{
            //    Console.WriteLine(item.Value.Accuracy);
            //}
            AccuracyWB bestAccuracy = AccuracyWeightB.OrderByDescending(x => x.Value.Accuracy).ThenByDescending(y => y.Key).Select(z => z.Value).First();


            Accuracy = bestAccuracy.Accuracy;
            BestWeightBias = bestAccuracy.Weight_Bias;
            Learning_Rate = learning_rate;
            //Console.WriteLine("\n" + Accuracy); 
        }
        public Data(StreamReader r1, StreamReader r2, double learning_rate, WeightBias bestWB)
        {
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            AccuracyWeightB = new Dictionary<int, AccuracyWB>();
            Predictions = new List<Prediction>();

            SetData(r1);
            perceptron = new Perceptron(Training_Data, null, learning_rate, false, 0, null, false, 0, false, 0, false);
            Accuracy = perceptron.GetAccuracy(Training_Data, bestWB);
            SetAccountIDs(r2, perceptron.Labels);            
        }
        public Data(StreamReader r1)
        {
            Training_Data = new List<Entry>();
            SetData(r1);
            int count = 0;
            foreach (var item in Training_Data)
            {
                if(item.Sign == 1)
                {
                    count++;
                }
            }
            double majority = (Convert.ToDouble(count) / Training_Data.Count) * 100;
            if(majority < 50)
            {
                majority = 100 - majority;
            }
            Majority = majority;
        }
        public void SetData(StreamReader reader, StreamReader reader_2 = null)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int Sign;
                double[] Vector = new double[16];
                string[] splitstring = line.Split();
                if(splitstring.First().First() == '1') { Sign = +1; }
                else { Sign = -1; }
                foreach (var item in splitstring)
                {
                    if (item.Contains(":"))
                    {
                        string[] s = item.Split(':');
                        Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                    }
                }
                Training_Data.Add(new Entry(Sign, Vector));
            }
            if (reader_2 != null)
            {
                reader_2.DiscardBufferedData();
                reader_2.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                string line2;
                while ((line2 = reader_2.ReadLine()) != null)
                {
                    int Sign;
                    double[] Vector = new double[16];
                    string[] splitstring = line2.Split();
                    if (splitstring.First().First() == '1') { Sign = +1; }
                    else { Sign = -1; }
                    foreach (var item in splitstring)
                    {
                        if (item.Contains(":"))
                        {
                            string[] s = item.Split(':');
                            Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                        }
                    }
                    Test_Data.Add(new Entry(Sign, Vector));
                }
            }
        }
        public void SetAccountIDs(StreamReader ids, List<int> labels)
        {
            ids.DiscardBufferedData();
            ids.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            int i = 0;
            string id;
            while ((id = ids.ReadLine()) != null)
            {
                Predictions.Add(new Prediction(Convert.ToInt32(id),  labels[i]));
                i++;
            }            
        }
        

        public void SetValidateData(StreamReader reader, StreamReader reader_2, Random rSeed)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int Sign;
                double[] Vector = new double[16];
                string[] splitstring = line.Split();
                if (splitstring.First().First() == '1') { Sign = +1; }
                else { Sign = -1; }
                foreach (var item in splitstring)
                {
                    if (item.Contains(":"))
                    {
                        string[] s = item.Split(':');
                        Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                    }
                }
                Cross_Validate_Data.Add(new Entry(Sign, Vector));
            }
            reader_2.DiscardBufferedData();
            reader_2.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            string line2;
            while ((line2 = reader_2.ReadLine()) != null)
            {
                int Sign;
                double[] Vector = new double[16];
                string[] splitstring = line2.Split();
                if (splitstring.First().First() == '1') { Sign = +1; }
                else { Sign = -1; }
                foreach (var item in splitstring)
                {
                    if (item.Contains(":"))
                    {
                        string[] s = item.Split(':');
                        Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                    }
                }
                Cross_Validate_Data.Add(new Entry(Sign, Vector));
            }
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            int seperator = Convert.ToInt32(Math.Floor(Convert.ToDecimal(Cross_Validate_Data.Count) / 5M));            
            Cross_1 = Cross_Validate_Data.GetRange(0, seperator);
            Cross_2 = Cross_Validate_Data.GetRange(seperator, seperator);
            Cross_3 = Cross_Validate_Data.GetRange(2 * seperator, seperator);
            Cross_4 = Cross_Validate_Data.GetRange(3 * seperator, seperator);
            Cross_5 = Cross_Validate_Data.GetRange(4 * seperator, Cross_Validate_Data.Count - (4 * seperator));
        }
    }
}
