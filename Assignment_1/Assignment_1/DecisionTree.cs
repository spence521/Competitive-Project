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
        public char? Value { get; set; }
        public List<TrainingData> trainingData { get; private set; }
        public double Entropy { get; private set; }
        public InformationGain informationGain { get; private set; }
        public double Error { get; set; } // gets set in SetEntropy
        public int DepthRemaining { get; set; }
        public List<Features> FeaturesTaken { get; set; }

        public DecisionTree(ref List<TrainingData> trainingdata, int depth)
        {
            informationGain = new InformationGain();
            FeaturesTaken = new List<Features>();
            IsLeaf = false;
            Children = new List<DecisionTree>();
            Value = null;
            DepthRemaining = depth;
            trainingData = trainingdata;
            SetEntropy();
            SetInformationGain();
            DetermineFeature();
            FeaturesTaken.Add(Features.None);
            FeaturesTaken.Add(Feature);
            DetermineSubTrees();
        }

        public DecisionTree(bool isLeaf, ref List<TrainingData> trainingdata, char? value, int depthRemaining, ref List<Features> featuresTaken)
        {
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
                Console.WriteLine("You have a NaN, or infinity value");
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
                if (item.TweetsPerDay == TrainingData.Tweets.Range0_66)
                {
                    if (item.Label == 1) { F11_Y_List[0]++; }
                    else { F11_N_List[0]++; }
                    F11_List_Total[0]++;
                }
                else if (item.TweetsPerDay == TrainingData.Tweets.Range67_132)
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

                H_Positive.Add(List_Total[i] == 0 ? 2 : P_List[i] / List_Total[i]);
                H_Negative.Add(List_Total[i] == 0 ? 2 : N_List[i] / List_Total[i]);
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
                Console.WriteLine("You have a NaN, or infinity value");
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
            , , Days, Hours, Minutes, Seconds, Following, Followers, Ratio, TotalTweets,
            TweetsPerDay, AverageLinks, AverageUniqueLinks, AverageUsername, AverageUniqueUsername, ChangeRate, None

            List<int?> LeafValues = new List<int?>();
            List<List<TrainingData>> Datas = new List<List<TrainingData>>();
            List<bool> Is_Leaf = new List<bool>();
            List<List<int>> Distinct_Labels = new List<List<int>>();
            //char? LeftLeafValue = null;
            //List<TrainingData> LeftData;
            var values = Enum.GetValues(typeof(Features));
            foreach (var item in values)
            {

            }
            if (Feature == Features.ScreenNameLength)
            {
                int length = Enum.GetNames(typeof(TrainingData.ScreenNameLength)).Length;
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range0_3).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range4_6).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range7_9).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.Range10_12).ToList());
                Datas.Add(trainingData.Where(x => x.screenNameLength == TrainingData.ScreenNameLength.RangeGT_12).ToList());

                Distinct_Labels.Add((from h in Datas[0] select h.Label).Distinct().ToList());
                Is_Leaf.Add(Distinct_Labels[0].Count == 1);
                if(Is_Leaf[0]) { LeafValues.Add(Datas[0].Select(p => p.Label).First()); }
                else if (Datas[0].Count < 2)
                {
                    Is_Leaf[0] = true;
                    if (Datas[0].Count == 0) { LeafValues.Add(0); }
                    else { LeafValues.Add(Datas[0].Select(p => p.Label).First()); }
                }
                else { LeafValues.Add(-1); } //A negative one leaf value mean its not a leaf 
            }
            else if (Feature == Features.DescriptionLength) { LeftData = trainingData.Where(x => x.DescriptionLength == false).ToList(); }
            else if (Feature == Features.FirstStartEnd) { LeftData = trainingData.Where(x => x.FirstStartEnd == false).ToList(); }
            else if (Feature == Features.FirstAlpha) { LeftData = trainingData.Where(x => x.FirstAlpha == false).ToList(); }
            else if (Feature == Features.FirstVowel) { LeftData = trainingData.Where(x => x.FirstVowel == false).ToList(); }
            else { LeftData = trainingData.Where(x => x.LastEven == false).ToList(); }
            
            List<char> LeftLeafList = (from h in LeftData select h.Label).Distinct().ToList();
            bool IsLeftLeaf = (LeftLeafList.Count == 1);
            if (IsLeftLeaf)
            {
                LeftLeafValue = LeftData.Select(p => p.Label).First();
            }
            else if(LeftData.Count < 2)
            {
                if (LeftData.Count == 0)
                {
                    LeftLeafValue = '-';
                    IsLeftLeaf = true;
                }
                else
                {
                    IsLeftLeaf = true;
                    LeftLeafValue = LeftData.Select(p => p.Label).First();
                }
            }

            List<bool> FirstBigger_LeftLeafList = (from h in LeftData select h.FirstBigger).Distinct().ToList();
            List<bool> MiddleName_LeftLeafList = (from h in LeftData select h.MiddleName).Distinct().ToList();
            List<bool> FirstStartEnd_LeftLeafList = (from h in LeftData select h.FirstStartEnd).Distinct().ToList();
            List<bool> FirstAlpha_LeftLeafList = (from h in LeftData select h.FirstAlpha).Distinct().ToList();
            List<bool> FirstVowel_LeftLeafList = (from h in LeftData select h.FirstVowel).Distinct().ToList();
            List<bool> LastEven_LeftLeafList = (from h in LeftData select h.LastEven).Distinct().ToList();            

            if (FirstBigger_LeftLeafList.Count == 1 && MiddleName_LeftLeafList.Count == 1 && FirstStartEnd_LeftLeafList.Count == 1 && FirstAlpha_LeftLeafList.Count == 1 &&
                FirstVowel_LeftLeafList.Count == 1 && LastEven_LeftLeafList.Count == 1)
            {
                IsLeftLeaf = true;
                LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
            }


            char? RightLeafValue = null;
            List<TrainingData> RightData;
            if (Feature == Features.FirstBigger) { RightData = trainingData.Where(x => x.FirstBigger == true).ToList(); }
            else if (Feature == Features.MiddleName) { RightData = trainingData.Where(x => x.MiddleName == true).ToList(); }
            else if (Feature == Features.FirstStartEnd) { RightData = trainingData.Where(x => x.FirstStartEnd == true).ToList(); }
            else if (Feature == Features.FirstAlpha) { RightData = trainingData.Where(x => x.FirstAlpha == true).ToList(); }
            else if (Feature == Features.FirstVowel) { RightData = trainingData.Where(x => x.FirstVowel == true).ToList(); }
            else { RightData = trainingData.Where(x => x.LastEven == true).ToList(); }      





            List<char> RightLeafList = (from h in RightData select h.Label).Distinct().ToList();
            bool IsRightLeaf = (RightLeafList.Count == 1);
            if (IsRightLeaf)
            {
                RightLeafValue = RightData.Select(p => p.Label).First();
            }
            else if (RightData.Count < 2)
            {
                IsRightLeaf = true;
                if (RightData.Count == 0)
                {
                    RightLeafValue = '+';
                    IsRightLeaf = true;
                }
                else
                {
                    IsRightLeaf = true;
                    RightLeafValue = RightData.Select(p => p.Label).First();
                }
            }


            List<bool> FirstBigger_RightLeafList = (from h in RightData select h.FirstBigger).Distinct().ToList();
            List<bool> MiddleName_RightLeafList = (from h in RightData select h.MiddleName).Distinct().ToList();
            List<bool> FirstStartEnd_RightLeafList = (from h in RightData select h.FirstStartEnd).Distinct().ToList();
            List<bool> FirstAlpha_RightLeafList = (from h in RightData select h.FirstAlpha).Distinct().ToList();
            List<bool> FirstVowel_RightLeafList = (from h in RightData select h.FirstVowel).Distinct().ToList();
            List<bool> LastEven_RightLeafList = (from h in RightData select h.LastEven).Distinct().ToList();

            List<bool> FirstPeriod_RightLeafList = (from h in RightData select h.FirstPeriod).Distinct().ToList();
            List<bool> MiddlePeriod_RightLeafList = (from h in RightData select h.MiddlePeriod).Distinct().ToList();
            List<bool> FirstHyphen_RightLeafList = (from h in RightData select h.FirstHyphen).Distinct().ToList();
            List<bool> LastHyphen_RightLeafList = (from h in RightData select h.LastHyphen).Distinct().ToList();
            List<bool> FirstEven_RightLeafList = (from h in RightData select h.FirstEven).Distinct().ToList();
            List<bool> MiddleEven_RightLeafList = (from h in RightData select h.MiddleEven).Distinct().ToList();
            List<bool> FirstBigger4_RightLeafList = (from h in RightData select h.FirstBigger4).Distinct().ToList();
            List<bool> LastBigger4_RightLeafList = (from h in RightData select h.LastBigger4).Distinct().ToList();
            List<bool> LastStartEnd_RightLeafList = (from h in RightData select h.LastStartEnd).Distinct().ToList();
            List<bool> MiddleStartEnd_RightLeafList = (from h in RightData select h.MiddleStartEnd).Distinct().ToList();
            List<bool> Last2ndVowel_RightLeafList = (from h in RightData select h.Last2ndVowel).Distinct().ToList();
            List<bool> First3rdVowel_RightLeafList = (from h in RightData select h.First3rdVowel).Distinct().ToList();
            List<bool> Last3rdVowel_RightLeafList = (from h in RightData select h.Last3rdVowel).Distinct().ToList();
            List<bool> ThreeSpaces_RightLeafList = (from h in RightData select h.ThreeSpaces).Distinct().ToList();

            if (FirstBigger_RightLeafList.Count == 1 && MiddleName_RightLeafList.Count == 1 && FirstStartEnd_RightLeafList.Count == 1 && FirstAlpha_RightLeafList.Count == 1 &&
                FirstVowel_RightLeafList.Count == 1 && LastEven_RightLeafList.Count == 1 && FirstPeriod_RightLeafList.Count == 1 && MiddlePeriod_RightLeafList.Count == 1 &&
                FirstHyphen_RightLeafList.Count == 1 && LastHyphen_RightLeafList.Count == 1 && FirstEven_RightLeafList.Count == 1 && MiddleEven_RightLeafList.Count == 1 &&
                FirstBigger4_RightLeafList.Count == 1 && LastBigger4_RightLeafList.Count == 1 && LastStartEnd_RightLeafList.Count == 1 && MiddleStartEnd_RightLeafList.Count == 1 &&
                Last2ndVowel_RightLeafList.Count == 1 && First3rdVowel_RightLeafList.Count == 1 && Last3rdVowel_RightLeafList.Count == 1 && ThreeSpaces_RightLeafList.Count == 1)
            {
                IsRightLeaf = true;
                RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
            }
            if(FeaturesTaken.Count > 19)
            {
                IsLeaf = true;
                Children = null;
                Value = trainingData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                Feature = Features.None;
                return;
            }
            

            if (LeftLeafValue.Equals(RightLeafValue) && (!(LeftLeafValue == null) || !(RightLeafValue == null)))
            {
                IsLeaf = true;
                Children = null;
                Value = LeftLeafValue;
                Feature = Features.None;
            }
            else
            {
                List<Features> featuresTakenHelper = FeaturesTaken;
                if (DepthRemaining > 1)
                {
                    LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                }
                else
                {
                    if(IsLeftLeaf && IsRightLeaf)
                    {
                        LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                        RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    }
                    else if(IsLeftLeaf)
                    {
                        RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                        LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                        RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    }
                    else if(IsRightLeaf)
                    {
                        LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                        LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                        RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    }
                    else
                    {
                        if (LeftData.Count == 0 && RightData.Count == 0)
                        {
                            LeftLeafValue = '-';
                            RightLeafValue = '+';
                        }
                        else if (LeftData.Count == 0)
                        {
                            LeftLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                        }
                        else if (RightData.Count == 0)
                        {
                            RightLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                        }
                        else
                        {
                            LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                            RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                        }
                        LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                        RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    }
                }
            }
        }
        public void PrintInformationGain()
        {
            Console.WriteLine("Is their first name longer than their last name?");
            Console.WriteLine(informationGain.FirstBigger.ToString());

            Console.WriteLine("Do they have a middle name?");
            Console.WriteLine(informationGain.MiddleName.ToString());

            Console.WriteLine("Does their first name start and end with the same letter?");
            Console.WriteLine(informationGain.FirstStartEnd.ToString());

            Console.WriteLine("Does their first name come alphabetically before their last name?");
            Console.WriteLine(informationGain.FirstAlpha.ToString());

            Console.WriteLine("Is the second letter of their first name a vowel (a,e,i,o,u)?");
            Console.WriteLine(informationGain.FirstVowel.ToString());

            Console.WriteLine("Is the number of letters in their last name even?");
            Console.WriteLine(informationGain.LastEven.ToString());
        }
        public int DetermineDepth(int count)
        {
            if(IsLeaf)
            {
                return count++;
            }
            else
            {
                int r = LeftTree.DetermineDepth(count + 1);
                int p = RightTree.DetermineDepth(count + 1);
                return Math.Max(r, p);
            }
        }
        public void CollapseTree()
        {
            if (!IsLeaf)
            {
                if (LeftTree.IsLeaf && RightTree.IsLeaf)
                {
                    if (LeftTree.Value.Equals(RightTree.Value))
                    {
                        IsLeaf = true;
                        Value = LeftTree.Value;
                        LeftTree = null;
                        RightTree = null;
                        Feature = Features.None;
                    }
                }
                else
                {
                    LeftTree.CollapseTree();
                    RightTree.CollapseTree();
                    if (LeftTree.IsLeaf && RightTree.IsLeaf)
                    {
                        if (LeftTree.Value.Equals(RightTree.Value))
                        {
                            IsLeaf = true;
                            Value = LeftTree.Value;
                            LeftTree = null;
                            RightTree = null;
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
                LeftTree.TraverseTree();
                RightTree.TraverseTree();
            }
        }
        public int DetermineError(ref List<TrainingData> TestData)
        {
            int errors = 0;
            foreach (var item in TestData)
            { //for each item, i want to traverse down the tree
                errors += DetermineSubError(item);
            }
            return errors;
        }
        private int DetermineSubError(TrainingData item)
        {
            if (IsLeaf)
            {
                if (item.Label != Value) { return 1; }
                else { return 0; }
            }
            else
            {
                if (Feature == Features.FirstBigger)
                {
                    if (item.FirstBigger) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.MiddleName)
                {
                    if (item.MiddleName) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.FirstStartEnd)
                {
                    if (item.FirstStartEnd) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.FirstAlpha)
                {
                    if (item.FirstAlpha) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                } 
                else if (Feature == Features.FirstVowel)
                {
                    if (item.FirstVowel) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if(Feature == Features.LastEven) 
                {
                    if (item.LastEven) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }                                
                else if (Feature == Features.FirstPeriod)
                {
                    if (item.FirstPeriod) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.MiddlePeriod)
                {
                    if (item.MiddlePeriod) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.FirstHyphen)
                {
                    if (item.FirstHyphen) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.LastHyphen)
                {
                    if (item.LastHyphen) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.FirstEven)
                {
                    if (item.FirstEven) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.MiddleEven)
                {
                    if (item.MiddleEven) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.FirstBigger4)
                {
                    if (item.FirstBigger4) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.LastBigger4)
                {
                    if (item.LastBigger4) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.LastStartEnd)
                {
                    if (item.LastStartEnd) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.MiddleStartEnd)
                {
                    if (item.MiddleStartEnd) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.Last2ndVowel)
                {
                    if (item.Last2ndVowel) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.First3rdVowel)
                {
                    if (item.First3rdVowel) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else if (Feature == Features.Last3rdVowel)
                {
                    if (item.Last3rdVowel) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
                else //(Feature == Features.ThreeSpaces)
                {
                    if (item.ThreeSpaces) { return RightTree.DetermineSubError(item); }
                    else { return LeftTree.DetermineSubError(item); }
                }
            }
        }

    }
}
