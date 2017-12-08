using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using static System.Math;

namespace Assignment_1
{
    public class Data
    {
        //public StreamReader reader { get; set; }
       // public StreamReader reader_2 { get; set; }
        public List<Entry> data_1 { get; private set; }
        public List<Entry> data_2 { get; private set; }
        public List<TrainingData> Training_Data { get; private set; }
        public List<TrainingData> Test_Data { get; private set; }
        public List<Entry> Cross_Validate_Data { get; private set; }
        public List<Entry> Cross_1 { get; private set; }
        public List<Entry> Cross_2 { get; private set; }
        public List<Entry> Cross_3 { get; private set; }
        public List<Entry> Cross_4 { get; private set; }
        public List<Entry> Cross_5 { get; private set; }
        public DecisionTree Tree { get; set; }
        public DecisionTree Tree2 { get; set; }
        public DecisionTree Tree3 { get; set; }
        public DecisionTree Tree4 { get; set; }
        public DecisionTree Tree5 { get; set; }
        public List<Prediction> Predictions { get; set; }
        public List<Prediction> Predictions2 { get; set; }
        public List<Prediction> Predictions3 { get; set; }
        public List<Prediction> Predictions4 { get; set; }
        public List<Prediction> Predictions5 { get; set; }
        public List<Prediction> Predictions_Average { get; set; }        
        public List<double> Accuracies { get; set; }
        public double Accuracy { get; set; }
        public int Depth { get; set; }
        public double Error { get; set; }
        public double StandardDeviation { get; set; }
        public List<BaggedForest> Forest { get; set; }
        public List<TrainingData> Training_Data_Forest { get; set; }

        public Data(StreamReader r, StreamReader r2, StreamReader eval, StreamReader eval_ID, int depth, Random rand, int ForestSize)
        {
            Forest = new List<BaggedForest>();
            Training_Data_Forest = new List<TrainingData>();
            data_1 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            data_2 = new List<Entry>();
            Test_Data = new List<TrainingData>();
            SetData(r, r2);
            SetTrainingData();
            Training_Data_Forest = Training_Data;

            for (int i = 0; i < ForestSize; i++)
            {
                data_1 = new List<Entry>();
                Training_Data = new List<TrainingData>();
                data_2 = new List<Entry>();
                Test_Data = new List<TrainingData>();
                SetData(r, r2);
                SetTrainingData();
                List<TrainingData> trainingDataHelper = Training_Data_Forest.GetRange(0, 1500);
                Tree = new DecisionTree(ref trainingDataHelper, depth, rand);
                Tree.CollapseTree();
                List<TrainingData> testDataHelper = Test_Data;
                Error = (Convert.ToDouble(Tree.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
                Accuracy = 100 - Error;
                Depth = Tree.DetermineDepth(0);
                ShuffleForestData(rand);

                data_1 = new List<Entry>();
                data_2 = new List<Entry>();
                Training_Data = new List<TrainingData>();
                SetData(eval);
                SetTrainingData();
                Tree.Labels = new List<int>();
                trainingDataHelper = Training_Data;
                Tree.DetermineError(ref trainingDataHelper);
                Predictions = SetPredictions(eval_ID, Tree.Labels);

                Forest.Add(new BaggedForest(Accuracy, Predictions));

                //if(i % 3 == 0) { Console.WriteLine(i); }
            }
        }
        private void ShuffleForestData(Random rand)
        {
            Training_Data_Forest = Training_Data_Forest.OrderBy(i => rand.Next()).ToList();
        }
        public Data(StreamReader train, StreamReader test, StreamReader eval, StreamReader eval_ID, int depth, Random r)
        {
            double temp_error1;
            double temp_error2;
            double temp_error3; 
            double temp_error4;
            double temp_error5;
            Cross_Validate_Data = new List<Entry>();
            Predictions = new List<Prediction>();
            Predictions2 = new List<Prediction>();
            Predictions3 = new List<Prediction>();
            Predictions4 = new List<Prediction>();
            Predictions5 = new List<Prediction>();
            Predictions_Average = new List<Prediction>();
            Cross_1 = new List<Entry>();
            Cross_2 = new List<Entry>();
            Cross_3 = new List<Entry>();
            Cross_4 = new List<Entry>();
            Cross_5 = new List<Entry>();
            Accuracies = new List<double>();
            SetValidateData(train, test, r);

            #region First Fold
            data_1 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            data_2 = new List<Entry>();
            Test_Data = new List<TrainingData>();
            
            data_1 = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_4))).ToList();
            data_2 = Cross_5;
            SetTrainingData();

            List<TrainingData> trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, depth, r);
            Tree.CollapseTree();

            List<TrainingData> testDataHelper = Test_Data;
            temp_error1 = (Convert.ToDouble(Tree.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
            Tree.Accuracy = 100 - temp_error1;

            data_1 = new List<Entry>();
            data_2 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            SetData(eval);
            SetTrainingData();
            Tree.Labels = new List<int>();
            trainingDataHelper = Training_Data;
            Tree.DetermineError(ref trainingDataHelper);
            Predictions = SetPredictions(eval_ID, Tree.Labels);
            #endregion

            #region Second Fold
            Training_Data = new List<TrainingData>();
            Test_Data = new List<TrainingData>();

            data_1 = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_5))).ToList();
            data_2 = Cross_4;
            SetTrainingData();

            trainingDataHelper = Training_Data;
            Tree2 = new DecisionTree(ref trainingDataHelper, depth, r);
            Tree2.CollapseTree();
            testDataHelper = Test_Data;
            temp_error2 = (Convert.ToDouble(Tree2.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
            Tree2.Accuracy = 100 - temp_error2;

            data_1 = new List<Entry>();
            data_2 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            SetData(eval);
            SetTrainingData();
            Tree2.Labels = new List<int>();
            trainingDataHelper = Training_Data;
            Tree2.DetermineError(ref trainingDataHelper);
            Predictions2 = SetPredictions(eval_ID, Tree2.Labels);
            #endregion

            #region Third Fold
            Training_Data = new List<TrainingData>();
            Test_Data = new List<TrainingData>();

            data_1 = Cross_1.Concat(Cross_2.Concat(Cross_4.Concat(Cross_5))).ToList();
            data_2 = Cross_3;
            SetTrainingData();

            trainingDataHelper = Training_Data;
            Tree3 = new DecisionTree(ref trainingDataHelper, depth, r);
            Tree3.CollapseTree();
            testDataHelper = Test_Data;
            temp_error3 = (Convert.ToDouble(Tree3.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
            Tree3.Accuracy = 100 - temp_error3;

            data_1 = new List<Entry>();
            data_2 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            SetData(eval);
            SetTrainingData();
            Tree3.Labels = new List<int>();
            trainingDataHelper = Training_Data;
            Tree3.DetermineError(ref trainingDataHelper);
            Predictions3 = SetPredictions(eval_ID, Tree3.Labels);
            #endregion

            #region Fourth Fold
            Training_Data = new List<TrainingData>();
            Test_Data = new List<TrainingData>();

            data_1 = Cross_1.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            data_2 = Cross_2;
            SetTrainingData();

            trainingDataHelper = Training_Data;
            Tree4 = new DecisionTree(ref trainingDataHelper, depth, r);
            Tree4.CollapseTree();
            testDataHelper = Test_Data;
            temp_error4 = (Convert.ToDouble(Tree4.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
            Tree4.Accuracy = 100 - temp_error4;

            data_1 = new List<Entry>();
            data_2 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            SetData(eval);
            SetTrainingData();
            Tree4.Labels = new List<int>();
            trainingDataHelper = Training_Data;
            Tree4.DetermineError(ref trainingDataHelper);
            Predictions4 = SetPredictions(eval_ID, Tree4.Labels);
            #endregion

            #region Fifth Fold
            Training_Data = new List<TrainingData>();
            Test_Data = new List<TrainingData>();

            data_1 = Cross_2.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            data_2 = Cross_1;
            SetTrainingData();

            trainingDataHelper = Training_Data;
            Tree5 = new DecisionTree(ref trainingDataHelper, depth, r);
            Tree5.CollapseTree();
            testDataHelper = Test_Data;
            temp_error5 = (Convert.ToDouble(Tree5.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
            Tree5.Accuracy = 100 - temp_error5;

            data_1 = new List<Entry>();
            data_2 = new List<Entry>();
            Training_Data = new List<TrainingData>();
            SetData(eval);
            SetTrainingData();
            Tree5.Labels = new List<int>();
            trainingDataHelper = Training_Data;
            Tree5.DetermineError(ref trainingDataHelper);
            Predictions5 = SetPredictions(eval_ID, Tree5.Labels);
            #endregion

            SetAveragedPredictions();
            StandardDeviation = CalculateStandardDeviation(1-temp_error1, 1-temp_error2, 1-temp_error3, 1-temp_error4, 1-temp_error5);
            //Console.WriteLine(temp_error1);
            //Console.WriteLine(temp_error2);
            //Console.WriteLine(temp_error3);
            //Console.WriteLine(temp_error4);
            Error = (temp_error1 + temp_error2 + temp_error3 + temp_error4) / 4;
            Accuracies.Add(Tree.Accuracy);
            Accuracies.Add(Tree2.Accuracy);
            Accuracies.Add(Tree3.Accuracy);
            Accuracies.Add(Tree4.Accuracy);
            Accuracies.Add(Tree5.Accuracy);
            Accuracy = Accuracies.Average();
            Depth = Tree.DetermineDepth(0);
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
                if (splitstring.First().First() == '1') { Sign = 1; }
                else { Sign = 0; }
                foreach (var item in splitstring)
                {
                    if (item.Contains(":"))
                    {
                        string[] s = item.Split(':');
                        Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                    }
                }
                data_1.Add(new Entry(Sign, Vector));
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
                    if (splitstring.First().First() == '1') { Sign = 1; }
                    else { Sign = 0; }
                    foreach (var item in splitstring)
                    {
                        if (item.Contains(":"))
                        {
                            string[] s = item.Split(':');
                            Vector[Convert.ToInt32(s[0]) - 1] = Convert.ToDouble(s[1]);
                        }
                    }
                    data_2.Add(new Entry(Sign, Vector));
                }
            }
        }
        public void PrintData1()
        {
            int i = 1;
            foreach (var item in data_1)
            {
                Console.WriteLine(i + "\t" + item.Sign + " " + item.Vector.ToString());
                i++;
            }
        }
        public void PrintData2()
        {
            int i = 1;
            foreach (var item in data_2)
            {
                Console.WriteLine(i + "\t" + item.Sign + " " + item.Vector.ToString());
                i++;
            }
        }

        public void SetTrainingData()
        {
            foreach (var item in data_1)
            {
                Training_Data.Add(new TrainingData(ScreenNameLength(item.Vector[0]), DescriptionLength(item.Vector[1]), Days(item.Vector[2]), Hours(item.Vector[3]), 
                    MinSec(item.Vector[4]), MinSec(item.Vector[5]), Follow(item.Vector[6]), Follow(item.Vector[7]), Ratio(item.Vector[8]), Tweets(item.Vector[9]), 
                    TweetsPerDay(item.Vector[10]), AverageLinks(item.Vector[11]), AverageLinks(item.Vector[12]), AverageUsername(item.Vector[13]), 
                    AverageUsername(item.Vector[14]), ChangeRate(item.Vector[15]), item.Sign));
            }
            foreach (var item in data_2)
            {
                Test_Data.Add(new TrainingData(ScreenNameLength(item.Vector[0]), DescriptionLength(item.Vector[1]), Days(item.Vector[2]), Hours(item.Vector[3]),
                    MinSec(item.Vector[4]), MinSec(item.Vector[5]), Follow(item.Vector[6]), Follow(item.Vector[7]), Ratio(item.Vector[8]), Tweets(item.Vector[9]),
                    TweetsPerDay(item.Vector[10]), AverageLinks(item.Vector[11]), AverageLinks(item.Vector[12]), AverageUsername(item.Vector[13]),
                    AverageUsername(item.Vector[14]), ChangeRate(item.Vector[15]), item.Sign));
            }
        }
        public void PrintTrainingData()
        {
            int i = 1;
            foreach (var item in Training_Data)
            {
                Console.WriteLine(i.ToString() + "\t" + item.ToString());
                i++;
            }
        }
        private double CalculateStandardDeviation(double acc1, double acc2, double acc3, double acc4, double acc5)
        {
            List<double> list = new List<double>() { acc1, acc2, acc3, acc4, acc5 };
            double AverageOfValues = list.Average();
            double SumOfValues = list.Sum(r => Math.Pow(r - AverageOfValues, 2));
            return Math.Sqrt((SumOfValues) / (list.Count));
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
                if (splitstring.First().First() == '1') { Sign = 1; }
                else { Sign = 0; }
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
                if (splitstring.First().First() == '1') { Sign = 1; }
                else { Sign = 0; }
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

        public void TraverseTree()
        {
            Tree.TraverseTree();
        }
        public List<Prediction> SetPredictions(StreamReader ids, List<int> labels)
        {
            List<Prediction> predictions = new List<Prediction>();
            ids.DiscardBufferedData();
            ids.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            int i = 0;
            string id;
            while ((id = ids.ReadLine()) != null)
            {
                predictions.Add(new Prediction(Convert.ToInt32(id), labels[i]));
                i++;
            }
            return predictions;
        }
        public void SetAveragedPredictions()
        {
            for (int i = 0; i < Predictions.Count; i++)
            {
                List<int> items = new List<int> { Predictions[i].Label, Predictions2[i].Label, Predictions3[i].Label, Predictions4[i].Label, Predictions5[i].Label };
                int prediction = items.GroupBy(m => m).OrderByDescending(g => g.Count()).Select(g => g.Key).First();
                Predictions_Average.Add(new Prediction(Predictions[i].Id, prediction));
            }
        }
        public TrainingData.ScreenNameLength ScreenNameLength(double value)
        {
            if (value <= 5) { return TrainingData.ScreenNameLength.Range0_3; }
            else if (5 < value && value <= 6) { return TrainingData.ScreenNameLength.Range4_6; }
            else if (6 < value && value <= 7) { return TrainingData.ScreenNameLength.Range7_9; }
            else if (7 < value && value <= 10) { return TrainingData.ScreenNameLength.Range10_12; }
            else { return TrainingData.ScreenNameLength.RangeGT_12; }            
        }
        public TrainingData.DescriptionLength DescriptionLength(double value)
        {
            if (value <= 50) { return TrainingData.DescriptionLength.Range0_33; }
            else if (50 < value && value <= 100) { return TrainingData.DescriptionLength.Range34_66; }
            else if (100 < value && value <= 150) { return TrainingData.DescriptionLength.Range67_99; }
            else if (150 < value && value <= 200) { return TrainingData.DescriptionLength.Range100_132; }
            else { return TrainingData.DescriptionLength.RangeGT_132; }            
        }
        public TrainingData.LongevityDays Days(double value)
        {
            if (value <= 200) { return TrainingData.LongevityDays.Range0_200; }
            else if (200 < value && value <= 400) { return TrainingData.LongevityDays.Range201_400; }
            else if (400 < value && value <= 600) { return TrainingData.LongevityDays.Range401_600; }
            else if (600 < value && value <= 800) { return TrainingData.LongevityDays.Range601_800; }
            else { return TrainingData.LongevityDays.RangeGT_800; }
        }
        public TrainingData.LongevityHours Hours(double value)
        {
            if (value <= 6) { return TrainingData.LongevityHours.Range0_8; }
            else if (6 < value && value <= 18) { return TrainingData.LongevityHours.Range9_16; }
            else { return TrainingData.LongevityHours.RangeGT_16; }
        }
        public TrainingData.LongevityMinSec MinSec(double value)
        {
            if (value <= 15) { return TrainingData.LongevityMinSec.Range0_15; }
            else if (15 < value && value <= 30) { return TrainingData.LongevityMinSec.Range16_30; }
            else if (30 < value && value <= 45) { return TrainingData.LongevityMinSec.Range31_45; }
            else { return TrainingData.LongevityMinSec.RangeGT_45; }
        }
        public TrainingData.Follow Follow(double value) //***********//// GOOD
        {
            if (value <= 1000) { return TrainingData.Follow.Range0_100; }
            else if (1000 < value && value <= 2000) { return TrainingData.Follow.Range101_400; }
            else if (2000 < value && value <= 3000) { return TrainingData.Follow.Range401_1400; }
            else if (3000 < value && value <= 4000) { return TrainingData.Follow.Range1401_3000; }
            else if (4000 < value && value <= 5000) { return TrainingData.Follow.Range3001_10000; }
            else { return TrainingData.Follow.RangeGT_10000; }

        }
        public TrainingData.Ratio Ratio(double value) //************/// GOOD
        {
            if (value <= 3.5) { return TrainingData.Ratio.Range0_1; }
            else if (3.5 < value && value <= 7) { return TrainingData.Ratio.Range1_3; }
            else if (7 < value && value <= 10.5) { return TrainingData.Ratio.Range3_8; }
            else { return TrainingData.Ratio.RangeGT_8; }
        } 
        public TrainingData.Tweets Tweets(double value) //*****BEST*****//
        {
            //if (value <= 20) { return TrainingData.Tweets.Range0_66; }
            //else if (20 < value && value <= 40) { return TrainingData.Tweets.Range67_132; }
            //else { return TrainingData.Tweets.RangeGT_132; }

            if (value <= 50) { return TrainingData.Tweets.Range0_66; }
            else if (50 < value && value <= 250) { return TrainingData.Tweets.Range67_132; }
            else { return TrainingData.Tweets.RangeGT_132; }

        }
        public TrainingData.TweetsPerDay TweetsPerDay(double value) //*****BEST*****//
        {
            if (value <= 4) { return TrainingData.TweetsPerDay.Range0_66; }
            else if (4 < value && value <= 10) { return TrainingData.TweetsPerDay.Range67_132; }
            else { return TrainingData.TweetsPerDay.RangeGT_132; }

            //if (value <= 66) { return TrainingData.TweetsPerDay.Range0_66; }
            //else if (66 < value && value <= 132) { return TrainingData.TweetsPerDay.Range67_132; }
            //else { return TrainingData.TweetsPerDay.RangeGT_132; }
        }
        public TrainingData.AverageLinks AverageLinks(double value) //*****BEST*****//
        {
            if (value <= 1) { return TrainingData.AverageLinks.Range0_1; }
            else if (1 < value && value <= 2) { return TrainingData.AverageLinks.Range1_2; }
            else { return TrainingData.AverageLinks.RangeGT_2; }
        }
        public TrainingData.AverageUsername AverageUsername(double value) //*****BEST*****//
        {
            if (value <= 2) { return TrainingData.AverageUsername.Range0_2; }
            else if (2 < value && value <= 4) { return TrainingData.AverageUsername.Range2_4; }
            else { return TrainingData.AverageUsername.RangeGT_4; }
        }
        public TrainingData.ChangeRate ChangeRate(double value) //*****BEST*****//
        {
            if (value <= 0.25) { return TrainingData.ChangeRate.Range0_5; }
            else if (0.25 < value && value <= 1) { return TrainingData.ChangeRate.Range5_50; }
            else { return TrainingData.ChangeRate.RangeGT_50; }
        }
    }
}
