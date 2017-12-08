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
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            Data data9;
            StreamReader Train;
            StreamReader Test;
            StreamReader Eval;
            StreamReader Eval_ID;
            Random r = new Random(2);
            

            #region Delete CSV's
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Decision_Tree_1 = startupPath + @"\Decision_Tree_1.csv";
            string Decision_Tree_2 = startupPath + @"\Decision_Tree_2.csv";
            string Decision_Tree_3 = startupPath + @"\Decision_Tree_3.csv";
            string Decision_Tree_4 = startupPath + @"\Decision_Tree_4.csv";
            string Decision_Tree_5 = startupPath + @"\Decision_Tree_5.csv";
            string Decision_Tree_Average = startupPath + @"\Decision_Tree_Average.csv";
            if (File.Exists(Decision_Tree_1)) { File.Delete(Decision_Tree_1); }
            if (File.Exists(Decision_Tree_2)) { File.Delete(Decision_Tree_2); }
            if (File.Exists(Decision_Tree_3)) { File.Delete(Decision_Tree_3); }
            if (File.Exists(Decision_Tree_4)) { File.Delete(Decision_Tree_4); }
            if (File.Exists(Decision_Tree_5)) { File.Delete(Decision_Tree_5); }
            if (File.Exists(Decision_Tree_Average)) { File.Delete(Decision_Tree_Average); }
            #endregion


            #region Passing in parameters
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a file argument.");
                return;
            }
            else if (args.Length > 3) //at least four arguments
            {
                //Train = File.OpenText(startupPath + @"\data.train");
                //Test = File.OpenText(startupPath + @"\data.test");
                //Eval = File.OpenText(startupPath + @"\data.eval.anon");
                //Train_ID = File.OpenText(startupPath + @"\data.train.id");
                //Test_ID = File.OpenText(startupPath + @"\data.test.id");
                //Eval_ID = File.OpenText(startupPath + @"\data.eval.id");
                Train = File.OpenText(args[0]);
                Test = File.OpenText(args[1]);
                Eval = File.OpenText(args[2]);
                Eval_ID = File.OpenText(args[3]);
                data2 = new Data(Train, Test, Eval, Eval_ID, 2, r);
                data3 = new Data(Train, Test, Eval, Eval_ID, 3, r);
                data4 = new Data(Train, Test, Eval, Eval_ID, 4, r);
                data5 = new Data(Train, Test, Eval, Eval_ID, 5, r);
                data6 = new Data(Train, Test, Eval, Eval_ID, 6, r);
                data7 = new Data(Train, Test, Eval, Eval_ID, 7, r);
                data8 = new Data(Train, Test, Eval, Eval_ID, 8, r);
                data9 = new Data(Train, Test, Eval, Eval_ID, 9, r);
                List<Data> ListOfDatas = new List<Data>() { data2, data3, data4, data5, data6, data7, data8, data9 };
                Data LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                int Depth = LargestData.Depth;
                int ForestSize = 1000;
                Data DataTree = new Data(Train, Test, Eval, Eval_ID, Depth, r, ForestSize);

                List<Prediction> FinalPredictions = new List<Prediction>();
                for (int i = 0; i < DataTree.Forest[0].Predictions.Count; i++)
                {
                    List<int> helper = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper.Add(tree.Predictions[i].Label);
                    }
                    int Most_Occured_Label = helper.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    int ID = DataTree.Forest[1].Predictions[i].Id;
                    FinalPredictions.Add(new Prediction(ID, Most_Occured_Label));
                }           

                GenerateCSV(FinalPredictions, "Bagged_Forest.csv");
                Console.WriteLine(DataTree.Depth);
            }
            #endregion          
        }
        static void GenerateCSV(List<Prediction> predictions, string name)
        {
            StringBuilder csv = new StringBuilder();
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Path = startupPath  + name;
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