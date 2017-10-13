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
                Console.WriteLine("\t" + Math.Round(100 - data.Error, 3) + "% Accuracy\n"
                    + "Standard Deviation:\t" + Math.Round(data.StandardDeviation, 4));
                //data.PrintData2();
            }

            #endregion

            #region Non-arguments
            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //StreamReader reader = File.OpenText(startupPath + @"\updated_training00.txt");
            //StreamReader reader2 = File.OpenText(startupPath + @"\updated_training01.txt");
            //StreamReader reader3 = File.OpenText(startupPath + @"\updated_training02.txt");
            //StreamReader reader4 = File.OpenText(startupPath + @"\updated_training03.txt");
            //StreamReader reader = File.OpenText(startupPath + @"\updated_train.txt");
            //StreamReader reader2 = File.OpenText(startupPath + @"\updated_test.txt");
            //data = new Data(reader, reader2);
            //Console.WriteLine(data.Error);
            //Console.ReadKey(false);
            #endregion
            //tree.PrintTrainingData();
            //tree.PrintData();
            //tree.display();
        }
        //static decimal helper(double P, double N)
        //{
        //    return decimal.Parse(((-P * Math.Log(P, 2)) - (N * Math.Log(N, 2))).ToString());
        //}
    }


}