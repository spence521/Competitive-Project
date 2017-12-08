using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Data tweetData;
            Data data;
            Data data1;
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            StreamReader Train;
            StreamReader Test;
            StreamReader Eval;
            StreamReader Train_ID;
            StreamReader Test_ID;
            StreamReader Eval_ID;
            StreamReader Tweets;
            Random r = new Random(2);

            #region Delete CSV's
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Decision_Tree = startupPath + @"\Decision_Tree.csv";
            string Best_Predictions = startupPath + @"\Best_Predictions.csv";
            if (File.Exists(Decision_Tree)) { File.Delete(Decision_Tree); }
            if (File.Exists(Best_Predictions)) { File.Delete(Best_Predictions); }
            #endregion


            #region Passing in parameters
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a file argument.");
                return;
            }
            else if (args.Length > 3) //at least four arguments
            {
                Train = File.OpenText(args[0]);
                Test = File.OpenText(args[1]);
                Eval = File.OpenText(args[2]);
                Train_ID = File.OpenText(args[3]);
                Test_ID = File.OpenText(args[4]);
                Eval_ID = File.OpenText(args[5]);
                //Tweets = File.OpenText(args[6]);

                //Train = File.OpenText(startupPath + @"\data.train");
                //Test = File.OpenText(startupPath + @"\data.test");
                //Eval = File.OpenText(startupPath + @"\data.eval.anon");
                //Train_ID = File.OpenText(startupPath + @"\data.train.id");
                //Test_ID = File.OpenText(startupPath + @"\data.test.id");
                //Eval_ID = File.OpenText(startupPath + @"\data.eval.id");
                Console.WriteLine(startupPath);
                Tweets = File.OpenText(startupPath + @"\Data_Files\tweets.txt");
                tweetData = new Data(Train, Test, Eval, Train_ID, Test_ID, Eval_ID, Tweets, r);
                List<ReTweets> re_tweets = tweetData.ReTweet;
                List<Post> post = null; // tweetData.Post;
                List<Entry> C1 = tweetData.Cross_1;
                List<Entry> C2 = tweetData.Cross_2;
                List<Entry> C3 = tweetData.Cross_3;
                List<Entry> C4 = tweetData.Cross_4;
                List<Entry> C5 = tweetData.Cross_5;
                List<Entry> Train_Data = tweetData.data_1;
                List<Entry> Test_Data = tweetData.data_2;
                List<Entry> Eval_Data = tweetData.data_3;
                data = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, int.MaxValue, r);
                data1 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 2, r);
                data2 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 3, r);
                data3 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 4, r);
                data4 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 5, r);
                data5 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 6, r);
                data6 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 7, r);
                data7 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 8, r);
                data8 = new Data(ref C1, ref C2, ref C3, ref C4, ref C5, ref Eval_Data, Eval_ID, ref re_tweets, ref post, 9, r);
                List<Data> ListofDatas = new List<Data>() { data, data1, data2, data3, data4, data5, data6, data7, data8 };
                Data BestData = ListofDatas.OrderByDescending(x => x.Accuracy).First();
                int depth = BestData.Depth;
                //Data(ref List<Entry> train, ref List<Entry> test, ref List<Entry> eval, StreamReader eval_ID, ref List<ReTweets> tweets, Random rand, int depth)
                Data DataTree = new Data(ref Train_Data, ref Test_Data, ref Eval_Data, Eval_ID, ref re_tweets, ref post, r, depth);

                Console.WriteLine("The Best Cross Validation Accuracy is:\t" + BestData.Accuracy);
                Console.WriteLine("The Best Hyper Parameter is:\t\nDepth:\t" + BestData.Depth);

                Console.WriteLine("The Test Accuracy for the Best Depth is:\t" + DataTree.Accuracy);
                Console.WriteLine("\t" + Math.Round(100 - data.Error, 3) + "% Accuracy\n"
                        + "Standard Deviation:\t" + Math.Round(data.StandardDeviation, 4));

                List<PredictionAccuracy> ListPredAcc = new List<PredictionAccuracy>();
                foreach (var item in ListofDatas)
                {
                    ListPredAcc.Add(new PredictionAccuracy(item.Predictions, item.Accuracies[0]));
                    ListPredAcc.Add(new PredictionAccuracy(item.Predictions2, item.Accuracies[1]));
                    ListPredAcc.Add(new PredictionAccuracy(item.Predictions3, item.Accuracies[2]));
                    ListPredAcc.Add(new PredictionAccuracy(item.Predictions4, item.Accuracies[3]));
                    ListPredAcc.Add(new PredictionAccuracy(item.Predictions5, item.Accuracies[4]));
                }
                GenerateCSV(DataTree.Predictions, @"\Decision_Tree.csv");
                PredictionAccuracy bestPrediction = ListPredAcc.OrderByDescending(x => x.Accuracy).FirstOrDefault();
                Console.WriteLine("The accuracy for the best tree generated from the cross validations (from about 45 trees) is:\t" + bestPrediction.Accuracy);

                GenerateCSV(bestPrediction.Predictions, @"\Best_Predictions.csv");

                //foreach (var data_item in ListofDatas)
                //{
                //    Console.WriteLine(data_item.Depth);
                //    foreach (var item in data_item.Accuracies)
                //    {
                //        Console.WriteLine(item);
                //    }
                //    Console.WriteLine();
                //    Console.WriteLine();
                //    Console.WriteLine();
                //}
                //Console.WriteLine(data.Depth);
                Console.ReadKey(false);
            }

            #endregion

            #region Non-arguments
            //Train = File.OpenText(startupPath + @"\data.train");
            //Test = File.OpenText(startupPath + @"\data.test");
            //Eval = File.OpenText(startupPath + @"\data.eval.anon");
            //Eval_ID = File.OpenText(startupPath + @"\data.eval.id");
            //data = new Data(Train, Test, Eval, Eval_ID, depth, r);
            //GenerateCSV(data.Predictions, @"\Simple_Perceptron.csv");
            //Console.WriteLine("\t" + Math.Round(100 - data.Error, 3) + "% Accuracy\n"
            //    + "Standard Deviation:\t" + Math.Round(data.StandardDeviation, 4));
            //foreach (var item in data.Accuracy)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(data.Depth);
            ////data.PrintData2();
            //Console.ReadKey(false);
            #endregion
            //tree.PrintTrainingData();
            //tree.PrintData();
            //tree.display();
        }
        static void GenerateCSV(List<Prediction> predictions, string name)
        {
            StringBuilder csv = new StringBuilder();
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Path = startupPath + name;
            csv.AppendLine("Id,Prediction");

            foreach (var item in predictions)
            {
                csv.AppendLine(string.Format("{0},{1}", item.Id, item.Label));
            }
            File.AppendAllText(Path, csv.ToString());
        }
        //static decimal helper(double P, double N)
        //{
        //    return decimal.Parse(((-P * Math.Log(P, 2)) - (N * Math.Log(N, 2))).ToString());
        //}
    }


}