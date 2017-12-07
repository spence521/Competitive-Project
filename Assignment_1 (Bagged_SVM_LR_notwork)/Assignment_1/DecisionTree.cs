using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class DecisionTree
    {
        /// <summary>
        /// Enums
        /// </summary>        
        public enum Features
        {
            ScreenNameLength, DescriptionLength, Days, Hours, Minutes, Seconds, Following, Followers, Ratio, TotalTweets,
            TweetsPerDay, AverageLinks, AverageUniqueLinks, AverageUsername, AverageUniqueUsername, ChangeRate, None
        }
        public Features Feature { get; set; }
        public bool IsLeaf { get; set; }
        public List<DecisionTree> Children { get; private set; }
        //public DecisionTree LeftTree { get; set; } //no
        //public DecisionTree RightTree { get; set; } //yes
        public int Value { get; set; }
        public List<TrainingData> trainingData { get; private set; }
        public double Entropy { get; private set; }
        public InformationGain informationGain { get; private set; }
        public double Error { get; set; } // gets set in SetEntropy
        public int DepthRemaining { get; set; }
        public List<Features> FeaturesTaken { get; set; }
        public double Accuracy { get; set; }
        public List<int> Labels { get; set; }
        public Random random { get; set; }

        public DecisionTree(ref List<TrainingData> trainingdata, int depth, Random r)
        {
            random = r;
            Labels = new List<int>();
            informationGain = new InformationGain();
            FeaturesTaken = new List<Features>();
            IsLeaf = false;
            Children = new List<DecisionTree>();
            Value = -1; //A negative one leaf value mean its not a leaf. 1 is positive label, 0 is negative label
            DepthRemaining = depth;
            trainingData = trainingdata;
            SetEntropy();
            SetInformationGain();
            DetermineFeature();
            FeaturesTaken.Add(Features.None);
            FeaturesTaken.Add(Feature);
            DetermineSubTrees();
        }

        public DecisionTree(bool isLeaf, ref List<TrainingData> trainingdata, int value, int depthRemaining, ref List<Features> featuresTaken, Random r)
        {
            random = r;
            IsLeaf = isLeaf;
            Children = new List<DecisionTree>();
            DepthRemaining = depthRemaining;
            Value = value;
            if (!IsLeaf)
            {
                FeaturesTaken = new List<Features>();
                informationGain = new InformationGain();
                trainingData = trainingdata;
                SetEntropy();
                SetInformationGain();
                FeaturesTaken = featuresTaken;
                DetermineFeature();
                FeaturesTaken.Add(Feature);
                DetermineSubTrees();
            }
            //else
            //{
            //    Console.WriteLine("You are at a Leaf" + value);
            //}
        }
        private void SetEntropy()
        {
            int positive_labels = 0;
            int negative_labels = 0;
            double P;
            double N;
            foreach (var item in trainingData)
            {
                if (item.Label == 1) { positive_labels++; }
                else { negative_labels++; }
            }
            P = Convert.ToDouble(positive_labels) / trainingData.Count;
            N = Convert.ToDouble(negative_labels) / trainingData.Count;
            Entropy = (-P * Math.Log(P, 2)) - (N * Math.Log(N, 2));
            Error = 1 - (Convert.ToDouble(positive_labels > negative_labels ? positive_labels : negative_labels) / Convert.ToDouble(trainingData.Count));

            if (double.IsInfinity(Entropy) || double.IsNaN(Entropy) || double.IsNegativeInfinity(Entropy) || double.IsPositiveInfinity(Entropy))
            {
                Console.WriteLine("You have a NaN, or infinity value. SetEntropy");
            }
        }
        private void SetInformationGain()
        {
            #region Features Values
            //Feature 1 values
            List<double> F1_Y_List = new List<double> { 0, 0, 0, 0, 0};
            List<double> F1_N_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F1_List_Total = new List<double> { 0, 0, 0, 0, 0 };
            //Feature 2 values
            List<double> F2_Y_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F2_N_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F2_List_Total = new List<double> { 0, 0, 0, 0, 0 };
            //Feature 3 values
            List<double> F3_Y_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F3_N_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F3_List_Total = new List<double> { 0, 0, 0, 0, 0 };
            //Feature 4 values
            List<double> F4_Y_List = new List<double> { 0, 0, 0 };
            List<double> F4_N_List = new List<double> { 0, 0, 0 };
            List<double> F4_List_Total = new List<double> { 0, 0, 0 };
            //Feature 5 values
            List<double> F5_Y_List = new List<double> { 0, 0, 0, 0 };
            List<double> F5_N_List = new List<double> { 0, 0, 0, 0 };
            List<double> F5_List_Total = new List<double> { 0, 0, 0, 0 };
            //Feature 6 values
            List<double> F6_Y_List = new List<double> { 0, 0, 0, 0};
            List<double> F6_N_List = new List<double> { 0, 0, 0, 0 };
            List<double> F6_List_Total = new List<double> { 0, 0, 0, 0 };
            //Feature 7 values
            List<double> F7_Y_List = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<double> F7_N_List = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<double> F7_List_Total = new List<double> { 0, 0, 0, 0, 0, 0 };
            //Feature 8 values
            List<double> F8_Y_List = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<double> F8_N_List = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<double> F8_List_Total = new List<double> { 0, 0, 0, 0, 0, 0 };
            //Feature ratio values
            List<double> F9_Y_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F9_N_List = new List<double> { 0, 0, 0, 0, 0 };
            List<double> F9_List_Total = new List<double> { 0, 0, 0, 0, 0 };
            //Feature totaltweets values
            List<double> F10_Y_List = new List<double> { 0, 0, 0};
            List<double> F10_N_List = new List<double> { 0, 0, 0};
            List<double> F10_List_Total = new List<double> { 0, 0, 0 };
            //Feature tweets per day values
            List<double> F11_Y_List = new List<double> { 0, 0, 0};
            List<double> F11_N_List = new List<double> { 0, 0, 0};
            List<double> F11_List_Total = new List<double> { 0, 0, 0 };
            //Feature average links values
            List<double> F12_Y_List = new List<double> { 0, 0, 0 };
            List<double> F12_N_List = new List<double> { 0, 0, 0 };
            List<double> F12_List_Total = new List<double> { 0, 0, 0 };
            //Feature average unique links values
            List<double> F13_Y_List = new List<double> { 0, 0, 0 };
            List<double> F13_N_List = new List<double> { 0, 0, 0 };
            List<double> F13_List_Total = new List<double> { 0, 0, 0};
            //Feature average username values
            List<double> F14_Y_List = new List<double> { 0, 0, 0};
            List<double> F14_N_List = new List<double> { 0, 0, 0};
            List<double> F14_List_Total = new List<double> { 0, 0, 0 };
            //Feature average unique username values
            List<double> F15_Y_List = new List<double> { 0, 0, 0};
            List<double> F15_N_List = new List<double> { 0, 0, 0};
            List<double> F15_List_Total = new List<double> { 0, 0, 0 };
            //Feature change rate values
            List<double> F16_Y_List = new List<double> { 0, 0, 0};
            List<double> F16_N_List = new List<double> { 0, 0, 0};
            List<double> F16_List_Total = new List<double> { 0, 0, 0 };
            #endregion

            #region Calculating Count Values
            foreach (var item in trainingData)
            {
                //Feature 1 counts
                if (item.screenNameLength == TrainingData.ScreenNameLength.Range0_3)
                {
                    if (item.Label == 1) { F1_Y_List[0]++; }
                    else { F1_N_List[0]++; }
                    F1_List_Total[0]++;
                }
                else if (item.screenNameLength == TrainingData.ScreenNameLength.Range4_6)
                {
                    if (item.Label == 1) { F1_Y_List[1]++; }
                    else { F1_N_List[1]++; }
                    F1_List_Total[1]++;
                }
                else if (item.screenNameLength == TrainingData.ScreenNameLength.Range7_9)
                {
                    if (item.Label == 1) { F1_Y_List[2]++; }
                    else { F1_N_List[2]++; }
                    F1_List_Total[2]++;
                }
                else if (item.screenNameLength == TrainingData.ScreenNameLength.Range10_12)
                {
                    if (item.Label == 1) { F1_Y_List[3]++; }
                    else { F1_N_List[3]++; }
                    F1_List_Total[3]++;
                }
                else
                {
                    if (item.Label == 1) { F1_Y_List[4]++; }
                    else { F1_N_List[4]++; }
                    F1_List_Total[4]++;
                }


                //Feature 2 
                if (item.descriptionLength == TrainingData.DescriptionLength.Range0_33)
                {
                    if (item.Label == 1) { F2_Y_List[0]++; }
                    else { F2_N_List[0]++; }
                    F2_List_Total[0]++;
                }
                else if (item.descriptionLength == TrainingData.DescriptionLength.Range34_66)
                {
                    if (item.Label == 1) { F2_Y_List[1]++; }
                    else { F2_N_List[1]++; }
                    F2_List_Total[1]++;
                }
                else if (item.descriptionLength == TrainingData.DescriptionLength.Range67_99)
                {
                    if (item.Label == 1) { F2_Y_List[2]++; }
                    else { F2_N_List[2]++; }
                    F2_List_Total[2]++;
                }
                else if (item.descriptionLength == TrainingData.DescriptionLength.Range100_132)
                {
                    if (item.Label == 1) { F2_Y_List[3]++; }
                    else { F2_N_List[3]++; }
                    F2_List_Total[3]++;
                }
                else
                {
                    if (item.Label == 1) { F2_Y_List[4]++; }
                    else { F2_N_List[4]++; }
                    F2_List_Total[4]++;
                }

                //Feature 3 
                if (item.Days == TrainingData.LongevityDays.Range0_200)
                {
                    if (item.Label == 1) { F3_Y_List[0]++; }
                    else { F3_N_List[0]++; }
                    F3_List_Total[0]++;
                }
                else if (item.Days == TrainingData.LongevityDays.Range201_400)
                {
                    if (item.Label == 1) { F3_Y_List[1]++; }
                    else { F3_N_List[1]++; }
                    F3_List_Total[1]++;
                }
                else if (item.Days == TrainingData.LongevityDays.Range401_600)
                {
                    if (item.Label == 1) { F3_Y_List[2]++; }
                    else { F3_N_List[2]++; }
                    F3_List_Total[2]++;
                }
                else if (item.Days == TrainingData.LongevityDays.Range601_800)
                {
                    if (item.Label == 1) { F3_Y_List[3]++; }
                    else { F3_N_List[3]++; }
                    F3_List_Total[3]++;
                }
                else
                {
                    if (item.Label == 1) { F3_Y_List[4]++; }
                    else { F3_N_List[4]++; }
                    F3_List_Total[4]++;
                }

                //Feature 4 
                if (item.Hours == TrainingData.LongevityHours.Range0_8)
                {
                    if (item.Label == 1) { F4_Y_List[0]++; }
                    else { F4_N_List[0]++; }
                    F4_List_Total[0]++;
                }
                else if (item.Hours == TrainingData.LongevityHours.Range9_16)
                {
                    if (item.Label == 1) { F4_Y_List[1]++; }
                    else { F4_N_List[1]++; }
                    F4_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F4_Y_List[2]++; }
                    else { F4_N_List[2]++; }
                    F4_List_Total[2]++;
                }


                //Feature 5
                if (item.Minutes == TrainingData.LongevityMinSec.Range0_15)
                {
                    if (item.Label == 1) { F5_Y_List[0]++; }
                    else { F5_N_List[0]++; }
                    F5_List_Total[0]++;
                }
                else if (item.Minutes == TrainingData.LongevityMinSec.Range16_30)
                {
                    if (item.Label == 1) { F5_Y_List[1]++; }
                    else { F5_N_List[1]++; }
                    F5_List_Total[1]++;
                }
                else if (item.Minutes == TrainingData.LongevityMinSec.Range31_45)
                {
                    if (item.Label == 1) { F5_Y_List[2]++; }
                    else { F5_N_List[2]++; }
                    F5_List_Total[2]++;
                }
                else
                {
                    if (item.Label == 1) { F5_Y_List[3]++; }
                    else { F5_N_List[3]++; }
                    F5_List_Total[3]++;
                }


                //Feature 6
                if (item.Seconds == TrainingData.LongevityMinSec.Range0_15)
                {
                    if (item.Label == 1) { F6_Y_List[0]++; }
                    else { F6_N_List[0]++; }
                    F6_List_Total[0]++;
                }
                else if (item.Seconds == TrainingData.LongevityMinSec.Range16_30)
                {
                    if (item.Label == 1) { F6_Y_List[1]++; }
                    else { F6_N_List[1]++; }
                    F6_List_Total[1]++;
                }
                else if (item.Seconds == TrainingData.LongevityMinSec.Range31_45)
                {
                    if (item.Label == 1) { F6_Y_List[2]++; }
                    else { F6_N_List[2]++; }
                    F6_List_Total[2]++;
                }
                else
                {
                    if (item.Label == 1) { F6_Y_List[3]++; }
                    else { F6_N_List[3]++; }
                    F6_List_Total[3]++;
                }

                //Feature 7
                if (item.Following == TrainingData.Follow.Range0_100)
                {
                    if (item.Label == 1) { F7_Y_List[0]++; }
                    else { F7_N_List[0]++; }
                    F7_List_Total[0]++;
                }
                else if (item.Following == TrainingData.Follow.Range101_400)
                {
                    if (item.Label == 1) { F7_Y_List[1]++; }
                    else { F7_N_List[1]++; }
                    F7_List_Total[1]++;
                }
                else if (item.Following == TrainingData.Follow.Range401_1400)
                {
                    if (item.Label == 1) { F7_Y_List[2]++; }
                    else { F7_N_List[2]++; }
                    F7_List_Total[2]++;
                }
                else if (item.Following == TrainingData.Follow.Range1401_3000)
                {
                    if (item.Label == 1) { F7_Y_List[3]++; }
                    else { F7_N_List[3]++; }
                    F7_List_Total[3]++;
                }
                else if (item.Following == TrainingData.Follow.Range3001_10000)
                {
                    if (item.Label == 1) { F7_Y_List[4]++; }
                    else { F7_N_List[4]++; }
                    F7_List_Total[4]++;
                }
                else
                {
                    if (item.Label == 1) { F7_Y_List[5]++; }
                    else { F7_N_List[5]++; }
                    F7_List_Total[5]++;
                }

                //Feature 8
                if (item.Followers == TrainingData.Follow.Range0_100)
                {
                    if (item.Label == 1) { F8_Y_List[0]++; }
                    else { F8_N_List[0]++; }
                    F8_List_Total[0]++;
                }
                else if (item.Followers == TrainingData.Follow.Range101_400)
                {
                    if (item.Label == 1) { F8_Y_List[1]++; }
                    else { F8_N_List[1]++; }
                    F8_List_Total[1]++;
                }
                else if (item.Followers == TrainingData.Follow.Range401_1400)
                {
                    if (item.Label == 1) { F8_Y_List[2]++; }
                    else { F8_N_List[2]++; }
                    F8_List_Total[2]++;
                }
                else if (item.Followers == TrainingData.Follow.Range1401_3000)
                {
                    if (item.Label == 1) { F8_Y_List[3]++; }
                    else { F8_N_List[3]++; }
                    F8_List_Total[3]++;
                }
                else if (item.Followers == TrainingData.Follow.Range3001_10000)
                {
                    if (item.Label == 1) { F8_Y_List[4]++; }
                    else { F8_N_List[4]++; }
                    F8_List_Total[4]++;
                }
                else
                {
                    if (item.Label == 1) { F8_Y_List[5]++; }
                    else { F8_N_List[5]++; }
                    F8_List_Total[5]++;
                }


                //Feature 9
                if (item.ratio == TrainingData.Ratio.Range0_1)
                {
                    if (item.Label == 1) { F9_Y_List[0]++; }
                    else { F9_N_List[0]++; }
                    F9_List_Total[0]++;
                }
                else if (item.ratio == TrainingData.Ratio.Range1_3)
                {
                    if (item.Label == 1) { F9_Y_List[1]++; }
                    else { F9_N_List[1]++; }
                    F9_List_Total[1]++;
                }
                else if (item.ratio == TrainingData.Ratio.Range3_8)
                {
                    if (item.Label == 1) { F9_Y_List[2]++; }
                    else { F9_N_List[2]++; }
                    F9_List_Total[2]++;
                }
                else
                {
                    if (item.Label == 1) { F9_Y_List[3]++; }
                    else { F9_N_List[3]++; }
                    F9_List_Total[3]++;
                }
                
                //Feature 10
                if (item.TotalTweets == TrainingData.Tweets.Range0_66)
                {
                    if (item.Label == 1) { F10_Y_List[0]++; }
                    else { F10_N_List[0]++; }
                    F10_List_Total[0]++;
                }
                else if (item.TotalTweets == TrainingData.Tweets.Range67_132)
                {
                    if (item.Label == 1) { F10_Y_List[1]++; }
                    else { F10_N_List[1]++; }
                    F10_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F10_Y_List[2]++; }
                    else { F10_N_List[2]++; }
                    F10_List_Total[2]++;
                }

                //Feature 11
                if (item.tweetsPerDay == TrainingData.TweetsPerDay.Range0_66)
                {
                    if (item.Label == 1) { F11_Y_List[0]++; }
                    else { F11_N_List[0]++; }
                    F11_List_Total[0]++;
                }
                else if (item.tweetsPerDay == TrainingData.TweetsPerDay.Range67_132)
                {
                    if (item.Label == 1) { F11_Y_List[1]++; }
                    else { F11_N_List[1]++; }
                    F11_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F11_Y_List[2]++; }
                    else { F11_N_List[2]++; }
                    F11_List_Total[2]++;
                }

                //Feature 12
                if (item.averageLinks == TrainingData.AverageLinks.Range0_1)
                {
                    if (item.Label == 1) { F12_Y_List[0]++; }
                    else { F12_N_List[0]++; }
                    F12_List_Total[0]++;
                }
                else if (item.averageLinks == TrainingData.AverageLinks.Range1_2)
                {
                    if (item.Label == 1) { F12_Y_List[1]++; }
                    else { F12_N_List[1]++; }
                    F12_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F12_Y_List[2]++; }
                    else { F12_N_List[2]++; }
                    F12_List_Total[2]++;
                }

                //Feature 13
                if (item.AverageUniqueLinks == TrainingData.AverageLinks.Range0_1)
                {
                    if (item.Label == 1) { F13_Y_List[0]++; }
                    else { F13_N_List[0]++; }
                    F13_List_Total[0]++;
                }
                else if (item.AverageUniqueLinks == TrainingData.AverageLinks.Range1_2)
                {
                    if (item.Label == 1) { F13_Y_List[1]++; }
                    else { F13_N_List[1]++; }
                    F13_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F13_Y_List[2]++; }
                    else { F13_N_List[2]++; }
                    F13_List_Total[2]++;
                }

                //Feature 14
                if (item.averageUsername == TrainingData.AverageUsername.Range0_2)
                {
                    if (item.Label == 1) { F14_Y_List[0]++; }
                    else { F14_N_List[0]++; }
                    F14_List_Total[0]++;
                }
                else if (item.averageUsername == TrainingData.AverageUsername.Range2_4)
                {
                    if (item.Label == 1) { F14_Y_List[1]++; }
                    else { F14_N_List[1]++; }
                    F14_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F14_Y_List[2]++; }
                    else { F14_N_List[2]++; }
                    F14_List_Total[2]++;
                }

                //Feature 15
                if (item.AverageUniqueUsername == TrainingData.AverageUsername.Range0_2)
                {
                    if (item.Label == 1) { F15_Y_List[0]++; }
                    else { F15_N_List[0]++; }
                    F15_List_Total[0]++;
                }
                else if (item.AverageUniqueUsername == TrainingData.AverageUsername.Range2_4)
                {
                    if (item.Label == 1) { F15_Y_List[1]++; }
                    else { F15_N_List[1]++; }
                    F15_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F15_Y_List[2]++; }
                    else { F15_N_List[2]++; }
                    F15_List_Total[2]++;
                }

                //Feature 16
                if (item.changeRate == TrainingData.ChangeRate.Range0_5)
                {
                    if (item.Label == 1) { F16_Y_List[0]++; }
                    else { F16_N_List[0]++; }
                    F16_List_Total[0]++;
                }
                else if (item.changeRate == TrainingData.ChangeRate.Range5_50)
                {
                    if (item.Label == 1) { F16_Y_List[1]++; }
                    else { F16_N_List[1]++; }
                    F16_List_Total[1]++;
                }
                else
                {
                    if (item.Label == 1) { F16_Y_List[2]++; }
                    else { F16_N_List[2]++; }
                    F16_List_Total[2]++;
                }

            }
            #endregion
                        
            //                                                      total yes/no's      Yes/no's for
            informationGain.ScreenNameLength = CalculateInformationGain(F1_List_Total, F1_Y_List, F1_N_List);
            informationGain.DescriptionLength = CalculateInformationGain(F2_List_Total, F2_Y_List, F2_N_List);
            informationGain.Days = CalculateInformationGain(F3_List_Total, F3_Y_List, F3_N_List);
            informationGain.Hours = CalculateInformationGain(F4_List_Total, F4_Y_List, F4_N_List);
            informationGain.Minutes = CalculateInformationGain(F5_List_Total, F5_Y_List, F5_N_List);
            informationGain.Seconds = CalculateInformationGain(F6_List_Total, F6_Y_List, F6_N_List);
            informationGain.Following = CalculateInformationGain(F7_List_Total, F7_Y_List, F7_N_List);
            informationGain.Followers = CalculateInformationGain(F8_List_Total, F8_Y_List, F8_N_List);
            informationGain.Ratio = CalculateInformationGain(F9_List_Total, F9_Y_List, F9_N_List);
            informationGain.TotalTweets = CalculateInformationGain(F10_List_Total, F10_Y_List, F10_N_List);
            informationGain.TweetsPerDay = CalculateInformationGain(F11_List_Total, F11_Y_List, F11_N_List);
            informationGain.AverageLinks = CalculateInformationGain(F12_List_Total, F12_Y_List, F12_N_List);
            informationGain.AverageUniqueLinks = CalculateInformationGain(F13_List_Total, F13_Y_List, F13_N_List);
            informationGain.AverageUsername = CalculateInformationGain(F14_List_Total, F14_Y_List, F14_N_List);
            informationGain.AverageUniqueUsername = CalculateInformationGain(F15_List_Total, F15_Y_List, F15_N_List);
            informationGain.ChangeRate = CalculateInformationGain(F16_List_Total, F16_Y_List, F16_N_List);
    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="List_Total">contains the total amount of each option</param>
        /// <param name="P_List">Contains the total amount of the positive results for each option</param>
        /// <param name="N_List">Contains the total amount of the negative results for each option</param>
        /// <returns></returns>
        private double CalculateInformationGain(List<double> List_Total, List<double> P_List, List<double> N_List)
        {
            List<double> Positive = new List<double>();
            List<double> Negative = new List<double>();
            List<double> H_Positive = new List<double>();
            List<double> H_Negative = new List<double>();
            List<double> Each_Result = new List<double>();

            for (int i = 0; i < List_Total.Count; i++)
            {
                Positive.Add(P_List[i] / (List_Total[i] == 0 ? 1 : List_Total[i]));
                Negative.Add(N_List[i] / (List_Total[i] == 0 ? 1 : List_Total[i]));

                H_Positive.Add(List_Total[i] == 0 ? 2 :  (P_List[i] == 0 ? 2 : (P_List[i] / List_Total[i])));
                H_Negative.Add(List_Total[i] == 0 ? 2 :  (N_List[i] == 0 ? 2 : (N_List[i] / List_Total[i])));
            }
            for (int i = 0; i < List_Total.Count; i++)
            {
                
                double insert = ((-Positive[i]) * Math.Log(H_Positive[i], 2)) - (Negative[i] * Math.Log(H_Negative[i], 2));
                Each_Result.Add(insert);
            }
            double Feature_Result = 0;
            for (int i = 0; i < List_Total.Count; i++)
            {
                Feature_Result += (List_Total[i] / Convert.ToDouble(trainingData.Count)) * Each_Result[i];
            }

            if(double.IsInfinity(Feature_Result) || double.IsNaN(Feature_Result) || double.IsNegativeInfinity(Feature_Result) || double.IsPositiveInfinity(Feature_Result))
            {
                Console.WriteLine("You have a NaN, or infinity value. Calc, Info Gain");
            }
            return Entropy - Feature_Result;           
        }
        private void DetermineFeature()
        {
            List<double> list = informationGain.ToList();
            if (list.Max() == list[0]) { Feature = Features.ScreenNameLength; }
            else if (list.Max() == list[1]) { Feature = Features.DescriptionLength; }
            else if (list.Max() == list[2]) { Feature = Features.Days; }
            else if (list.Max() == list[3]) { Feature = Features.Hours; }
            else if (list.Max() == list[4]) { Feature = Features.Minutes; }
            else if (list.Max() == list[5]) { Feature = Features.Seconds; }
            else if (list.Max() == list[6]) { Feature = Features.Following; }
            else if (list.Max() == list[7]) { Feature = Features.Followers; }
            else if (list.Max() == list[8]) { Feature = Features.Ratio; }
            else if (list.Max() == list[9]) { Feature = Features.TotalTweets; }
            else if (list.Max() == list[10]) { Feature = Features.TweetsPerDay; }
            else if (list.Max() == list[11]) { Feature = Features.AverageLinks; }
            else if (list.Max() == list[12]) { Feature = Features.AverageUniqueLinks; }
            else if (list.Max() == list[13]) { Feature = Features.AverageUsername; }
            else if (list.Max() == list[14]) { Feature = Features.AverageUniqueUsername; }
            else if (list.Max() == list[15]) { Feature = Features.ChangeRate; }
            else { Feature = Features.None; }

            if(list.All(x => x == 0))
            {
                if (!FeaturesTaken.Contains(Features.ScreenNameLength)) { Feature = Features.ScreenNameLength; }
                else if (!FeaturesTaken.Contains(Features.DescriptionLength)) { Feature = Features.DescriptionLength; }
                else if (!FeaturesTaken.Contains(Features.Days)) { Feature = Features.Days; }
                else if (!FeaturesTaken.Contains(Features.Hours)) { Feature = Features.Hours; }
                else if (!FeaturesTaken.Contains(Features.Minutes)) { Feature = Features.Minutes; }
                else if (!FeaturesTaken.Contains(Features.Seconds)) { Feature = Features.Seconds; }

                else if (!FeaturesTaken.Contains(Features.Following)) { Feature = Features.Following; }
                else if (!FeaturesTaken.Contains(Features.Followers)) { Feature = Features.Followers; }
                else if (!FeaturesTaken.Contains(Features.Ratio)) { Feature = Features.Ratio; }
                else if (!FeaturesTaken.Contains(Features.TotalTweets)) { Feature = Features.TotalTweets; }
                else if (!FeaturesTaken.Contains(Features.TweetsPerDay)) { Feature = Features.TweetsPerDay; }
                else if (!FeaturesTaken.Contains(Features.AverageLinks)) { Feature = Features.AverageLinks; }
                else if (!FeaturesTaken.Contains(Features.AverageUniqueLinks)) { Feature = Features.AverageUniqueLinks; }
                else if (!FeaturesTaken.Contains(Features.AverageUsername)) { Feature = Features.AverageUsername; }
                else if (!FeaturesTaken.Contains(Features.AverageUniqueUsername)) { Feature = Features.AverageUniqueUsername; }
                else if (!FeaturesTaken.Contains(Features.ChangeRate)) { Feature = Features.ChangeRate; }
                else { Feature = Features.None; }
            }
        }
        private void DetermineSubTrees()
        {
            List<int> LeafValues = new List<int>();
            List<List<TrainingData>> Datas = new List<List<TrainingData>>();
            List<bool> Is_Leaf = new List<bool>();
            List<List<int>> Distinct_Labels = new List<List<int>>();
            List<List<TrainingData.ScreenNameLength>> screenNameLength = new List<List<TrainingData.ScreenNameLength>>();
            List<List<TrainingData.DescriptionLength>> descriptionLength = new List<List<TrainingData.DescriptionLength>>();
            List<List<TrainingData.LongevityDays>> Days = new List<List<TrainingData.LongevityDays>>();
            List<List<TrainingData.LongevityHours>> Hours = new List<List<TrainingData.LongevityHours>>();
            List<List<TrainingData.LongevityMinSec>> Minutes = new List<List<TrainingData.LongevityMinSec>>();
            List<List<TrainingData.LongevityMinSec>> Seconds = new List<List<TrainingData.LongevityMinSec>>();
            List<List<TrainingData.Follow>> Following = new List<List<TrainingData.Follow>>();
            List<List<TrainingData.Follow>> Followers = new List<List<TrainingData.Follow>>();
            List<List<TrainingData.Ratio>> ratio = new List<List<TrainingData.Ratio>>();
            List<List<TrainingData.Tweets>> TotalTweets = new List<List<TrainingData.Tweets>>();
            List<List<TrainingData.TweetsPerDay>> TweetsPerDay = new List<List<TrainingData.TweetsPerDay>>();
            List<List<TrainingData.AverageLinks>> averageLinks = new List<List<TrainingData.AverageLinks>>();
            List<List<TrainingData.AverageLinks>> AverageUniqueLinks = new List<List<TrainingData.AverageLinks>>();
            List<List<TrainingData.AverageUsername>> averageUsername = new List<List<TrainingData.AverageUsername>>();
            List<List<TrainingData.AverageUsername>> AverageUniqueUsername = new List<List<TrainingData.AverageUsername>>();
            List<List<TrainingData.ChangeRate>> changeRate = new List<List<TrainingData.ChangeRate>>();
            //char? LeftLeafValue = null;
            //List<TrainingData> LeftData;
            if (Feature == Features.ScreenNameLength)
            {
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range0_3).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range4_6).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range7_9).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range10_12).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.RangeGT_12).ToList());

            }
            else if (Feature == Features.DescriptionLength)
            {
                Datas.Add(trainingData.Where(x => x.descriptionLength == TrainingData.DescriptionLength.Range0_33).ToList());
                Datas.Add(trainingData.Where(x => x.descriptionLength == TrainingData.DescriptionLength.Range34_66).ToList());
                Datas.Add(trainingData.Where(x => x.descriptionLength == TrainingData.DescriptionLength.Range67_99).ToList());
                Datas.Add(trainingData.Where(x => x.descriptionLength == TrainingData.DescriptionLength.Range100_132).ToList());
                Datas.Add(trainingData.Where(x => x.descriptionLength == TrainingData.DescriptionLength.RangeGT_132).ToList());
            }
            else if (Feature == Features.Days)
            {
                Datas.Add(trainingData.Where(x => x.Days == TrainingData.LongevityDays.Range0_200).ToList());
                Datas.Add(trainingData.Where(x => x.Days == TrainingData.LongevityDays.Range201_400).ToList());
                Datas.Add(trainingData.Where(x => x.Days == TrainingData.LongevityDays.Range401_600).ToList());
                Datas.Add(trainingData.Where(x => x.Days == TrainingData.LongevityDays.Range601_800).ToList());
                Datas.Add(trainingData.Where(x => x.Days == TrainingData.LongevityDays.RangeGT_800).ToList());
            }
            else if (Feature == Features.Hours)
            {
                Datas.Add(trainingData.Where(x => x.Hours == TrainingData.LongevityHours.Range0_8).ToList());
                Datas.Add(trainingData.Where(x => x.Hours == TrainingData.LongevityHours.Range9_16).ToList());
                Datas.Add(trainingData.Where(x => x.Hours == TrainingData.LongevityHours.RangeGT_16).ToList());
            }
            else if (Feature == Features.Minutes)
            {
                Datas.Add(trainingData.Where(x => x.Minutes == TrainingData.LongevityMinSec.Range0_15).ToList());
                Datas.Add(trainingData.Where(x => x.Minutes == TrainingData.LongevityMinSec.Range16_30).ToList());
                Datas.Add(trainingData.Where(x => x.Minutes == TrainingData.LongevityMinSec.Range31_45).ToList());
                Datas.Add(trainingData.Where(x => x.Minutes == TrainingData.LongevityMinSec.RangeGT_45).ToList());
            }
            else if (Feature == Features.Seconds)
            {
                Datas.Add(trainingData.Where(x => x.Seconds == TrainingData.LongevityMinSec.Range0_15).ToList());
                Datas.Add(trainingData.Where(x => x.Seconds == TrainingData.LongevityMinSec.Range16_30).ToList());
                Datas.Add(trainingData.Where(x => x.Seconds == TrainingData.LongevityMinSec.Range31_45).ToList());
                Datas.Add(trainingData.Where(x => x.Seconds == TrainingData.LongevityMinSec.RangeGT_45).ToList());
            }
            else if (Feature == Features.Following)
            {
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.Range0_100).ToList());
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.Range101_400).ToList());
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.Range401_1400).ToList());
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.Range1401_3000).ToList());
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.Range3001_10000).ToList());
                Datas.Add(trainingData.Where(x => x.Following == TrainingData.Follow.RangeGT_10000).ToList());
            }
            else if (Feature == Features.Followers)
            {
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.Range0_100).ToList());
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.Range101_400).ToList());
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.Range401_1400).ToList());
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.Range1401_3000).ToList());
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.Range3001_10000).ToList());
                Datas.Add(trainingData.Where(x => x.Followers == TrainingData.Follow.RangeGT_10000).ToList());
            }
            else if (Feature == Features.Ratio)
            {
                Datas.Add(trainingData.Where(x => x.ratio == TrainingData.Ratio.Range0_1).ToList());
                Datas.Add(trainingData.Where(x => x.ratio == TrainingData.Ratio.Range1_3).ToList());
                Datas.Add(trainingData.Where(x => x.ratio == TrainingData.Ratio.Range3_8).ToList());
                Datas.Add(trainingData.Where(x => x.ratio == TrainingData.Ratio.RangeGT_8).ToList());
            }
            else if (Feature == Features.TotalTweets)
            {
                Datas.Add(trainingData.Where(x => x.TotalTweets == TrainingData.Tweets.Range0_66).ToList());
                Datas.Add(trainingData.Where(x => x.TotalTweets == TrainingData.Tweets.Range67_132).ToList());
                Datas.Add(trainingData.Where(x => x.TotalTweets == TrainingData.Tweets.RangeGT_132).ToList());
            }
            else if (Feature == Features.TweetsPerDay)
            {
                Datas.Add(trainingData.Where(x => x.tweetsPerDay == TrainingData.TweetsPerDay.Range0_66).ToList());
                Datas.Add(trainingData.Where(x => x.tweetsPerDay == TrainingData.TweetsPerDay.Range67_132).ToList());
                Datas.Add(trainingData.Where(x => x.tweetsPerDay == TrainingData.TweetsPerDay.RangeGT_132).ToList());
            }
            else if (Feature == Features.AverageLinks)
            {
                Datas.Add(trainingData.Where(x => x.averageLinks == TrainingData.AverageLinks.Range0_1).ToList());
                Datas.Add(trainingData.Where(x => x.averageLinks == TrainingData.AverageLinks.Range1_2).ToList());
                Datas.Add(trainingData.Where(x => x.averageLinks == TrainingData.AverageLinks.RangeGT_2).ToList());
            }
            else if (Feature == Features.AverageUniqueLinks)
            {
                Datas.Add(trainingData.Where(x => x.AverageUniqueLinks == TrainingData.AverageLinks.Range0_1).ToList());
                Datas.Add(trainingData.Where(x => x.AverageUniqueLinks == TrainingData.AverageLinks.Range1_2).ToList());
                Datas.Add(trainingData.Where(x => x.AverageUniqueLinks == TrainingData.AverageLinks.RangeGT_2).ToList());
            }
            else if (Feature == Features.AverageUsername)
            {
                Datas.Add(trainingData.Where(x => x.averageUsername == TrainingData.AverageUsername.Range0_2).ToList());
                Datas.Add(trainingData.Where(x => x.averageUsername == TrainingData.AverageUsername.Range2_4).ToList());
                Datas.Add(trainingData.Where(x => x.averageUsername == TrainingData.AverageUsername.RangeGT_4).ToList());
            }
            else if (Feature == Features.AverageUniqueUsername)
            {
                Datas.Add(trainingData.Where(x => x.AverageUniqueUsername == TrainingData.AverageUsername.Range0_2).ToList());
                Datas.Add(trainingData.Where(x => x.AverageUniqueUsername == TrainingData.AverageUsername.Range2_4).ToList());
                Datas.Add(trainingData.Where(x => x.AverageUniqueUsername == TrainingData.AverageUsername.RangeGT_4).ToList());
            }
            else //change rate
            {
                Datas.Add(trainingData.Where(x => x.changeRate == TrainingData.ChangeRate.Range0_5).ToList());
                Datas.Add(trainingData.Where(x => x.changeRate == TrainingData.ChangeRate.Range5_50).ToList());
                Datas.Add(trainingData.Where(x => x.changeRate == TrainingData.ChangeRate.RangeGT_50).ToList());
            }

            for (int i = 0; i < Datas.Count; i++)
            {
                Distinct_Labels.Add((from h in Datas[i] select h.Label).Distinct().ToList());
                Is_Leaf.Add(Distinct_Labels[i].Count == 1);
                if (Is_Leaf[i]) { LeafValues.Add(Datas[i].Select(p => p.Label).First()); }
                else if (Datas[i].Count < 2)
                {
                    Is_Leaf[i] = true;
                    if (Datas[i].Count == 0) { LeafValues.Add(0); }
                    else { LeafValues.Add(Datas[i].Select(p => p.Label).First()); }
                }
                else { LeafValues.Add(-1); } //A negative one leaf value mean its not a leaf. 1 is positive label, 0 is negative label
            }

            foreach (var item in Datas)
            {
                screenNameLength.Add((from h in item select h.screenNameLength).Distinct().ToList()); // this determines if there are more than 1 of the same result from the feature 
                descriptionLength.Add((from h in item select h.descriptionLength).Distinct().ToList());
                Days.Add((from h in item select h.Days).Distinct().ToList());
                Hours.Add((from h in item select h.Hours).Distinct().ToList());
                Minutes.Add((from h in item select h.Minutes).Distinct().ToList());
                Seconds.Add((from h in item select h.Seconds).Distinct().ToList());
                Following.Add((from h in item select h.Following).Distinct().ToList());
                Followers.Add((from h in item select h.Followers).Distinct().ToList());
                ratio.Add((from h in item select h.ratio).Distinct().ToList());
                TotalTweets.Add((from h in item select h.TotalTweets).Distinct().ToList());
                TweetsPerDay.Add((from h in item select h.tweetsPerDay).Distinct().ToList());
                averageLinks.Add((from h in item select h.averageLinks).Distinct().ToList());
                AverageUniqueLinks.Add((from h in item select h.AverageUniqueLinks).Distinct().ToList());
                averageUsername.Add((from h in item select h.averageUsername).Distinct().ToList());
                AverageUniqueUsername.Add((from h in item select h.AverageUniqueUsername).Distinct().ToList());
                changeRate.Add((from h in item select h.changeRate).Distinct().ToList());
            }
            for (int i = 0; i < Datas.Count; i++)
            {
                if(screenNameLength[i].Count == 1 && descriptionLength[i].Count == 1 && Days[i].Count == 1 && Hours[i].Count == 1 && Minutes[i].Count == 1 &&
                    Seconds[i].Count == 1 && Following[i].Count == 1 && Followers[i].Count == 1 && ratio[i].Count == 1 && TotalTweets[i].Count == 1 && TweetsPerDay[i].Count == 1 
                    && averageLinks[i].Count == 1 && AverageUniqueLinks[i].Count == 1 && averageUsername[i].Count == 1 && AverageUniqueUsername[i].Count == 1 && changeRate[i].Count == 1)
                {
                    Is_Leaf[i] = true;
                    LeafValues[i] = Datas[i].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
            }            
            
            if(FeaturesTaken.Count > 15)
            {
                IsLeaf = true;
                Children = null;
                Value = trainingData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                Feature = Features.None;
                return;
            }
            
            if(LeafValues.Distinct().ToList().Count == 1 && LeafValues.Any(x => x != -1))
            {
                IsLeaf = true;
                Children = null;
                Value = LeafValues.First();
                Feature = Features.None;
            }
            else
            {
                List<Features> featuresTakenHelper = FeaturesTaken;
                if (DepthRemaining > 1)
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        List<TrainingData> data = Datas[i];
                        Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                    }
                }
                else
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        if (Is_Leaf[i])
                        {
                            List<TrainingData> data = Datas[i];
                            Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                        }
                        else
                        {
                            if (Datas[i].Count > 0)
                            {
                                LeafValues[i] = Datas[i].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                            }
                            else
                            {
                                if (Datas.Any(x => x.Count > 0)) //There is at least 1 list that contains data points
                                {
                                    List<TrainingData> sumofDatas = new List<TrainingData>();
                                    foreach (var item in Datas)
                                    {
                                        sumofDatas.AddRange(item);
                                    }
                                    LeafValues[i] = sumofDatas.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                                }
                                else //none of the lists contain data points, so get a random lab
                                {
                                    LeafValues[i] = (random.Next() % 2);
                                }
                            }
                            List<TrainingData> data = Datas[i];
                            Children.Add(new DecisionTree(true, ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                        }
                    }
                    //if (IsLeftLeaf && IsRightLeaf)
                    //{
                    //    LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else if (IsLeftLeaf)
                    //{
                    //    RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else if (IsRightLeaf)
                    //{
                    //    LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else
                    //{
                    //    if (LeftData.Count == 0 && RightData.Count == 0)
                    //    {
                    //        LeftLeafValue = '-';
                    //        RightLeafValue = '+';
                    //    }
                    //    else if (LeftData.Count == 0)
                    //    {
                    //        LeftLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    else if (RightData.Count == 0)
                    //    {
                    //        RightLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    else
                    //    {
                    //        LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //        RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                }
            }            
        }
        public void PrintInformationGain()
        {
            Console.WriteLine(informationGain.ToString());
        }
        public int DetermineDepth(int count)
        {
            if(IsLeaf)
            {
                return count++;
            }
            else
            {
                List<int> rp = new List<int>();
                foreach (var item in Children)
                {
                    rp.Add(item.DetermineDepth(count++));
                }
                return rp.Max();
            }
        }
        public void CollapseTree()
        {
            if (!IsLeaf)
            {
                if (Children.All(x => x.IsLeaf))
                {
                    int Children_Equal = Children.Select(x => x.Value).Distinct().ToList().Count;
                    if (Children_Equal == 1)
                    {
                        IsLeaf = true;
                        Value = Children[0].Value;
                        Children = null;
                        Feature = Features.None;
                    }
                }
                else
                {
                    foreach (var item in Children)
                    {
                        item.CollapseTree();
                    }
                    if (Children.All(x => x.IsLeaf))
                    {
                        int Children_Equal = Children.Select(x => x.Value).Distinct().ToList().Count;
                        if (Children_Equal == 1)
                        {
                            IsLeaf = true;
                            Value = Children[0].Value;
                            Children = null;
                            Feature = Features.None;
                        }
                    }
                }
            }
        }
        public void TraverseTree()
        {
            if(IsLeaf)
            {
                Console.WriteLine(Value);
            }
            else
            {
                Console.WriteLine(Feature);
                foreach (var item in Children)
                {
                    item.TraverseTree();
                }
            }
        }
        public int DetermineError(ref List<TrainingData> TestData)
        {
            int errors = 0;
            foreach (var item in TestData)
            { //for each item, i want to traverse down the tree
                List<int> helper = DetermineSubError(item);
                errors += helper.First();
                Labels.Add(helper.Last());
            }
            return errors;
        }
        private List<int> DetermineSubError(TrainingData item)
        {
            if (IsLeaf)
            {
                if (item.Label != Value) { return  new List<int> { 1, Value }; }
                else { return new List<int> { 0, Value }; }
            }
            else
            {   
                if (Feature == Features.ScreenNameLength)
                {
                    if (item.screenNameLength == TrainingData.ScreenNameLength.Range0_3) { return Children[0].DetermineSubError(item); }
                    else if (item.screenNameLength == TrainingData.ScreenNameLength.Range4_6) { return Children[1].DetermineSubError(item); }
                    else if (item.screenNameLength == TrainingData.ScreenNameLength.Range7_9) { return Children[2].DetermineSubError(item); }
                    else if (item.screenNameLength == TrainingData.ScreenNameLength.Range10_12) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.DescriptionLength)
                {
                    if (item.descriptionLength == TrainingData.DescriptionLength.Range0_33) { return Children[0].DetermineSubError(item); }
                    else if (item.descriptionLength == TrainingData.DescriptionLength.Range34_66) { return Children[1].DetermineSubError(item); }
                    else if (item.descriptionLength == TrainingData.DescriptionLength.Range67_99) { return Children[2].DetermineSubError(item); }
                    else if (item.descriptionLength == TrainingData.DescriptionLength.Range100_132) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.Days)
                {
                    if (item.Days == TrainingData.LongevityDays.Range0_200) { return Children[0].DetermineSubError(item); }
                    else if (item.Days == TrainingData.LongevityDays.Range201_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Days == TrainingData.LongevityDays.Range401_600) { return Children[2].DetermineSubError(item); }
                    else if (item.Days == TrainingData.LongevityDays.Range601_800) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.Hours)
                {
                    if (item.Hours == TrainingData.LongevityHours.Range0_8) { return Children[0].DetermineSubError(item); }
                    else if (item.Hours == TrainingData.LongevityHours.Range9_16) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                } 
                else if (Feature == Features.Minutes)
                {
                    if (item.Minutes == TrainingData.LongevityMinSec.Range0_15) { return Children[0].DetermineSubError(item); }
                    else if (item.Minutes == TrainingData.LongevityMinSec.Range16_30) { return Children[1].DetermineSubError(item); }
                    else if (item.Minutes == TrainingData.LongevityMinSec.Range31_45) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }
                else if(Feature == Features.Seconds)
                {
                    if (item.Seconds == TrainingData.LongevityMinSec.Range0_15) { return Children[0].DetermineSubError(item); }
                    else if (item.Seconds == TrainingData.LongevityMinSec.Range16_30) { return Children[1].DetermineSubError(item); }
                    else if (item.Seconds == TrainingData.LongevityMinSec.Range31_45) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }                                
                else if (Feature == Features.Following)
                {
                    if (item.Following == TrainingData.Follow.Range0_100) { return Children[0].DetermineSubError(item); }
                    else if (item.Following == TrainingData.Follow.Range101_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Following == TrainingData.Follow.Range401_1400) { return Children[2].DetermineSubError(item); }
                    else if (item.Following == TrainingData.Follow.Range1401_3000) { return Children[3].DetermineSubError(item); }
                    else if (item.Following == TrainingData.Follow.Range3001_10000) { return Children[4].DetermineSubError(item); }
                    else { return Children[5].DetermineSubError(item); }
                }
                else if (Feature == Features.Followers)
                {
                    if (item.Followers == TrainingData.Follow.Range0_100) { return Children[0].DetermineSubError(item); }
                    else if (item.Followers == TrainingData.Follow.Range101_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Followers == TrainingData.Follow.Range401_1400) { return Children[2].DetermineSubError(item); }
                    else if (item.Followers == TrainingData.Follow.Range1401_3000) { return Children[3].DetermineSubError(item); }
                    else if (item.Followers == TrainingData.Follow.Range3001_10000) { return Children[4].DetermineSubError(item); }
                    else { return Children[5].DetermineSubError(item); }
                }
                else if (Feature == Features.Ratio)
                {
                    if (item.ratio == TrainingData.Ratio.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.ratio == TrainingData.Ratio.Range1_3) { return Children[1].DetermineSubError(item); }
                    else if (item.ratio == TrainingData.Ratio.Range3_8) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }
                else if (Feature == Features.TotalTweets)
                {
                    if (item.TotalTweets == TrainingData.Tweets.Range0_66) { return Children[0].DetermineSubError(item); }
                    else if (item.TotalTweets == TrainingData.Tweets.Range67_132) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.TweetsPerDay)
                {
                    if (item.tweetsPerDay == TrainingData.TweetsPerDay.Range0_66) { return Children[0].DetermineSubError(item); }
                    else if (item.tweetsPerDay == TrainingData.TweetsPerDay.Range67_132) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageLinks)
                {
                    if (item.averageLinks == TrainingData.AverageLinks.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.averageLinks == TrainingData.AverageLinks.Range1_2) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUniqueLinks)
                {
                    if (item.AverageUniqueLinks == TrainingData.AverageLinks.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.AverageUniqueLinks == TrainingData.AverageLinks.Range1_2) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUsername)
                {
                    if (item.averageUsername == TrainingData.AverageUsername.Range0_2) { return Children[0].DetermineSubError(item); }
                    else if (item.averageUsername == TrainingData.AverageUsername.Range2_4) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUniqueUsername)
                {
                    if (item.AverageUniqueUsername == TrainingData.AverageUsername.Range0_2) { return Children[0].DetermineSubError(item); }
                    else if (item.AverageUniqueUsername == TrainingData.AverageUsername.Range2_4) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else // (Feature == Features.ChangeRate)
                {
                    if (item.changeRate == TrainingData.ChangeRate.Range0_5) { return Children[0].DetermineSubError(item); }
                    else if (item.changeRate == TrainingData.ChangeRate.Range5_50) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
            }
        }

    }
}
