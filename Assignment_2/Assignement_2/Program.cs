using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms.DataVisualization.Charting;

namespace Assignement_2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Data data1;
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            Data data9;
            #region Arguments
            StreamReader Train;
            StreamReader Test;
            StreamReader Eval;
            StreamReader Eval_ID;
            Random r = new Random(2);
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a file argument.");
                return;
            }
            else if (args.Length > 3)
            {
                Train = File.OpenText(args[0]);
                Test = File.OpenText(args[1]);
                Eval = File.OpenText(args[2]);
                Eval_ID = File.OpenText(args[3]);

                #region Majority Baseline
                //Data dataTest0 = new Data(Test);
                //Data dataDev0 = new Data(dev);
                //Console.WriteLine("\nThe Majority Baseline Accuracy on the Test set is: \n\t" + Math.Round(dataTest0.Majority, 3));
                //Console.WriteLine("The Majority Baseline Accuracy on the Development set is: \n\t" + Math.Round(dataDev0.Majority, 3));
                #endregion

                #region Delete CSV's
                string startupPath = System.IO.Directory.GetCurrentDirectory() + @"\CSV_Files";
                string simple = startupPath + @"\Simple_Perceptron.csv";
                string dynamic = startupPath + @"\Dynamic_Learning_Rate.csv";
                string margin_delete = startupPath + @"\Margin_Perceptron.csv";
                string averaged = startupPath + @"\Averaged_Perceptron.csv";
                string aggressive = startupPath + @"\Aggressive_Perceptron.csv";
                if (File.Exists(simple)) { File.Delete(simple); }
                if (File.Exists(dynamic)) { File.Delete(dynamic); }
                if (File.Exists(margin_delete)) { File.Delete(margin_delete); }
                if (File.Exists(averaged)) { File.Delete(averaged); }
                if (File.Exists(aggressive)) { File.Delete(aggressive); }
                #endregion

                #region Part 1
                Data dataEval;
                Data dataTest;
                data1 = new Data(10, 1, r, false, 0, false, false, Train, Test);
                data2 = new Data(10, 0.1, r, false, 0, false, false, Train, Test);
                data3 = new Data(10, 0.01, r, false, 0, false, false, Train, Test);
                List<Data> ListOfDatas = new List<Data>() { data1, data2, data3 };
                Data LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                double learning_rate = LargestData.Learning_Rate;
                Console.WriteLine("\n*******Simple Perceptron (Part 1)*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + Math.Round(learning_rate, 3));
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
                dataTest = new Data(Train, Test, 20, learning_rate, r, false, 0, false, false);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Accuracy, 3));
                dataEval = new Data(Eval, Eval_ID, learning_rate, dataTest.BestWeightBias);
                GenerateCSV(dataEval.Predictions, @"\Simple_Perceptron.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve part 1: \n\t");
                foreach (var item in dataTest.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Part 2
                Data dataEval2;
                Data dataTest2;
                data1 = new Data(10, 1, r, true, 0, false, false, Train, Test);
                data2 = new Data(10, 0.1, r, true, 0, false, false, Train, Test);
                data3 = new Data(10, 0.01, r, true, 0, false, false, Train, Test);
                ListOfDatas = new List<Data>() { data1, data2, data3 };
                LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                learning_rate = LargestData.Learning_Rate;
                Console.WriteLine("\n\n\n*******Perceptron with Dynamic Learning Rate (Part 2)*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + Math.Round(learning_rate, 3));
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
                dataTest2 = new Data(Train, Test, 20, learning_rate, r, true, 0, false, false);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest2.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest2.Accuracy, 3));
                dataEval2 = new Data(Eval, Eval_ID, learning_rate, dataTest2.BestWeightBias);
                GenerateCSV(dataEval2.Predictions, @"\Dynamic_Learning_Rate.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataTest2.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve part 2: \n\t");
                foreach (var item in dataTest2.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Part 3
                Data dataEval3;
                Data dataTest3;
                double margin;
                #region Data
                data1 = new Data(10, 1, r, true, 1, false, false, Train, Test);
                data2 = new Data(10, 0.1, r, true, 1, false, false, Train, Test);
                data3 = new Data(10, 0.01, r, true, 1, false, false, Train, Test);
                data4 = new Data(10, 1, r, true, 0.1, false, false, Train, Test);
                data5 = new Data(10, 0.1, r, true, 0.1, false, false, Train, Test);
                data6 = new Data(10, 0.01, r, true, 0.1, false, false, Train, Test);
                data7 = new Data(10, 1, r, true, 0.01, false, false, Train, Test);
                data8 = new Data(10, 0.1, r, true, 0.01, false, false, Train, Test);
                data9 = new Data(10, 0.01, r, true, 0.01, false, false, Train, Test);
                #endregion
                ListOfDatas = new List<Data>() { data1, data2, data3, data4, data5, data6, data7, data8, data9 };
                LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                learning_rate = LargestData.Learning_Rate;
                margin = LargestData.Margin;
                Console.WriteLine("\n\n\n*******Margin Perceptron (Part 3)*******");
                Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + Math.Round(learning_rate, 3) + "\n\tMargin:\t" + Math.Round(margin, 3));
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
                dataTest3 = new Data(Train, Test, 20, learning_rate, r, true, margin, false, false);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest3.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest3.Accuracy, 3));
                dataEval3 = new Data(Eval, Eval_ID, learning_rate, dataTest3.BestWeightBias);
                GenerateCSV(dataEval3.Predictions, @"\Margin_Perceptron.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval3.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve part 3: \n\t");
                foreach (var item in dataTest3.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Part 4
                Data dataEval4;
                Data dataTest4;
                data1 = new Data(10, 1, r, false, 0, true, false, Train, Test);
                data2 = new Data(10, 0.1, r, false, 0, true, false, Train, Test);
                data3 = new Data(10, 0.01, r, false, 0, true, false, Train, Test);
                ListOfDatas = new List<Data>() { data1, data2, data3 };
                LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                learning_rate = LargestData.Learning_Rate;
                Console.WriteLine("\n*******Averaged Perceptron (Part 4)*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + Math.Round(learning_rate, 3));
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
                dataTest4 = new Data(Train, Test, 20, learning_rate, r, false, 0, true, false);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest4.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest4.Accuracy, 3));
                dataEval4 = new Data(Eval, Eval_ID, learning_rate, dataTest4.BestWeightBias);
                GenerateCSV(dataEval4.Predictions, @"\Averaged_Perceptron.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval4.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve part 4: \n\t");
                foreach (var item in dataTest4.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Part 5
                Data dataEval5;
                Data dataTest5;
                data1 = new Data(10, 1, r, false, 1, false, true, Train, Test);
                data2 = new Data(10, 0.1, r, false, 0.1, false, true, Train, Test);
                data3 = new Data(10, 0.01, r, false, 0.01, false, true, Train, Test);
                ListOfDatas = new List<Data>() { data1, data2, data3 };
                LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                margin = LargestData.Margin;
                Console.WriteLine("\n\n\n*******Aggressive Perceptron with Margin (Part 5)*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Margin:\t" + Math.Round(margin, 3));
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
                dataTest5 = new Data(Train, Test, 20, learning_rate, r, true, margin, false, true);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest5.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest5.Accuracy, 3));
                dataEval5 = new Data(Eval, Eval_ID, learning_rate, dataTest5.BestWeightBias);
                GenerateCSV(dataEval5.Predictions, @"\Aggressive_Perceptron.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval5.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve part 5: \n\t");
                foreach (var item in dataTest5.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Chart
                //Chart chart = new Chart();
                //chart.Series.Clear();
                //chart.Size = new System.Drawing.Size(960, 480);
                //chart.ChartAreas.Add("ChartedAreas");
                //chart.ChartAreas["ChartedAreas"].AxisX.Minimum = 0;
                //chart.ChartAreas["ChartedAreas"].AxisX.Maximum = 21;
                //chart.ChartAreas["ChartedAreas"].AxisY.Minimum = 70;
                //chart.ChartAreas["ChartedAreas"].AxisY.Maximum = 100;
                //chart.ChartAreas["ChartedAreas"].AxisX.Interval = 1;
                //chart.ChartAreas["ChartedAreas"].AxisY.Interval = 3;
                //chart.Legends.Add("Legend1");
                //chart = GenerateChart(chart, dataTest, "Part 1");
                //chart = GenerateChart(chart, dataTest2, "Part 2");
                //chart = GenerateChart(chart, dataTest3, "Part 3");
                //chart = GenerateChart(chart, dataTest4, "Part 4");
                //chart = GenerateChart(chart, dataTest5, "Part 5");
                //chart.SaveImage("Chart.png", ChartImageFormat.Png);
                #endregion
            }
            #endregion

            #region Non-Arguments
            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //Train = File.OpenText(startupPath + @"\data.train");
            //Test = File.OpenText(startupPath + @"\data.test");
            //Eval = File.OpenText(startupPath + @"\data.eval.anon");
            //Eval_ID = File.OpenText(startupPath + @"\data.eval.id");

            //Data dataEval;
            //Data dataTest;
            //data1 = new Data(10, 1, r, false, 0, false, false, Train, Test);
            //data2 = new Data(10, 0.1, r, false, 0, false, false, Train, Test);
            //data3 = new Data(10, 0.01, r, false, 0, false, false, Train, Test);
            //List<Data> ListOfDatas = new List<Data>() { data1, data2, data3 };
            //Data LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
            //double learning_rate = LargestData.Learning_Rate;
            //Console.WriteLine("\n*******Simple Perceptron (Part 1)*******");
            //Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + Math.Round(learning_rate, 3));
            //Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Accuracy, 3));
            //dataTest = new Data(Train, Test, 20, learning_rate, r, false, 0, false, false);
            //Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
            //Console.WriteLine("Developement Set Accuracy: \n\t" + Math.Round(dataTest.Accuracy, 3));
            //dataEval = new Data(Eval, Eval_ID, learning_rate, dataTest.BestWeightBias);
            //GenerateCSV(dataEval.Predictions, @"\Simple_Perceptron.csv");
            //Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataEval.Accuracy, 3));
            //Console.WriteLine("The following are the accuracies from the learning curve part 1: \n\t");
            //foreach (var item in dataTest.AccuracyWeightB)
            //{
            //    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
            //}
            //Console.WriteLine("---------------------------------------------------------------------------------------");

            //Console.ReadKey(false);
            #endregion

        }
        #region Chart
        //static Chart GenerateChart(Chart chart, Data part1, string seriesName)
        //{
        //    chart.Series.Add(seriesName);
        //    chart.Series[seriesName].ChartType = SeriesChartType.Spline;
        //    chart.Series[seriesName].XValueType = ChartValueType.Int32; //the epoch ID
        //    chart.Series[seriesName].YValueType = ChartValueType.Double; //Development set Acuracy
        //    int i = 1;
        //    foreach (var item in part1.AccuracyWeightB)
        //    {
        //        chart.Series[seriesName].Points.AddXY(i, item.Value.Accuracy);
        //        i++;
        //    }
        //    chart.Series[seriesName].BorderWidth = 2;
        //    return chart;
        //}
        #endregion
        static void GenerateCSV(List<Prediction> predictions, string name)
        {
            StringBuilder csv = new StringBuilder();
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Path = startupPath + @"\CSV_Files" + name;
            csv.AppendLine("Id,Prediction");

            foreach (var item in predictions)
            {
                csv.AppendLine(string.Format("{0},{1}", item.Id, item.Label));
            }
            File.AppendAllText(Path, csv.ToString());
        }
    }
}
