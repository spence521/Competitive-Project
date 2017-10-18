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
            Data data;
            StreamReader Train;
            StreamReader Test;
            StreamReader Eval;
            StreamReader Eval_ID;
            Random r = new Random(2);
            int depth = int.MaxValue;

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
                Train = File.OpenText(args[0]);
                Test = File.OpenText(args[1]);
                Eval = File.OpenText(args[2]);
                Eval_ID = File.OpenText(args[3]);
                if (args.Length > 4) { depth = Convert.ToInt32(args[4]); }
                data = new Data(Train, Test, Eval, Eval_ID, depth, r);
                GenerateCSV(data.Predictions, @"\Decision_Tree_1.csv");
                GenerateCSV(data.Predictions2, @"\Decision_Tree_2.csv");
                GenerateCSV(data.Predictions3, @"\Decision_Tree_3.csv");
                GenerateCSV(data.Predictions4, @"\Decision_Tree_4.csv");
                GenerateCSV(data.Predictions5, @"\Decision_Tree_5.csv");
                GenerateCSV(data.Predictions_Average, @"\Decision_Tree_Average.csv");
                Console.WriteLine("\t" + Math.Round(100 - data.Error, 3) + "% Accuracy\n"
                    + "Standard Deviation:\t" + Math.Round(data.StandardDeviation, 4));
                foreach (var item in data.Accuracy)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine(data.Depth);
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