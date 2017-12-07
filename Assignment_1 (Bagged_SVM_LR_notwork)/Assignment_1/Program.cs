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
            Data data1;
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            Data data9;
            Data data10;
            Data data11;
            Data data12;
            Data data13;
            Data data14;
            Data data15;
            Data data16;
            Data data17;
            Data data18;
            Data data19;
            Data data20;
            Data data21;
            Data data22;
            Data data23;
            Data data24;
            Data data25;
            Data data26;
            Data data27;
            Data data28;
            Data data29;
            Data data30;
            Data data31;
            Data data32;
            Data data33;
            Data data34;
            Data data35;
            Data data36;
            StreamReader Train;
            StreamReader Test;
            StreamReader Eval;
            StreamReader Train_ID;
            StreamReader Test_ID;
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
            string Trees_SVM = startupPath + @"\Trees_SVM.csv";
            string Trees_LR = startupPath + @"\Trees_Logistic_Regression.csv";
            if (File.Exists(Decision_Tree_1)) { File.Delete(Decision_Tree_1); }
            if (File.Exists(Decision_Tree_2)) { File.Delete(Decision_Tree_2); }
            if (File.Exists(Decision_Tree_3)) { File.Delete(Decision_Tree_3); }
            if (File.Exists(Decision_Tree_4)) { File.Delete(Decision_Tree_4); }
            if (File.Exists(Decision_Tree_5)) { File.Delete(Decision_Tree_5); }
            if (File.Exists(Decision_Tree_Average)) { File.Delete(Decision_Tree_Average); }
            if (File.Exists(Trees_SVM)) { File.Delete(Trees_SVM); }
            if (File.Exists(Trees_LR)) { File.Delete(Trees_LR); }
            #endregion


            #region Passing in parameters
            //if (args.Length == 0)
            //{
            //    System.Console.WriteLine("Please enter a file argument.");
            //    return;
            //}
            //else if (args.Length > 3) //at least four arguments
            //{
                Train = File.OpenText(startupPath + @"\data.train");
                Test = File.OpenText(startupPath + @"\data.test");
                Eval = File.OpenText(startupPath + @"\data.eval.anon");
                Train_ID = File.OpenText(startupPath + @"\data.train.id");
                Test_ID = File.OpenText(startupPath + @"\data.test.id");
                Eval_ID = File.OpenText(startupPath + @"\data.eval.id");
            //Train = File.OpenText(args[0]);
            //Test = File.OpenText(args[1]);
            //Eval = File.OpenText(args[2]);
            //Train_ID = File.OpenText(args[3]);
            //Test_ID = File.OpenText(args[4]);
            //Eval_ID = File.OpenText(args[5]);
            //data2 = new Data(Train, Test, Eval, Eval_ID, 10, r);
            //data3 = new Data(Train, Test, Eval, Eval_ID, 11, r);
            //data4 = new Data(Train, Test, Eval, Eval_ID, 4, r);
            //data5 = new Data(Train, Test, Eval, Eval_ID, 5, r);
            //data6 = new Data(Train, Test, Eval, Eval_ID, 6, r);
            //data7 = new Data(Train, Test, Eval, Eval_ID, 7, r);
            //data8 = new Data(Train, Test, Eval, Eval_ID, 8, r);
            //data9 = new Data(Train, Test, Eval, Eval_ID, 9, r);
            //data10 = new Data(Train, Test, Eval, Eval_ID, 12, r);
            //data11 = new Data(Train, Test, Eval, Eval_ID, 13, r);
            List<Data> ListOfDatas;// = new List<Data>() { data2, data3, data4, data5, data6, data7, data8, data9, data10, data11 };
            Data LargestData;// = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
            int Depth = 7;// LargestData.Depth;
                Console.WriteLine("The best Hyperparemeters for Bagged Forest are:\n\tDepth:\t" + Depth);

                int ForestSize = 100;
                Data DataTree = new Data(Train, Test, Eval, Train_ID, Test_ID, Eval_ID, Depth, r, ForestSize);

                List<Entry> SVM_LR_Train_Data = new List<Entry>();
                List<Entry> SVM_LR_Test_Data = new List<Entry>();
                List<Entry> SVM_LR_Eval_Data = new List<Entry>();
                List<Prediction> FinalPredictions_Train = new List<Prediction>();
                List<Prediction> FinalPredictions_Test = new List<Prediction>();
                List<Prediction> FinalPredictions_Eval = new List<Prediction>();
                for (int i = 0; i < DataTree.Forest[0].Train_Predictions.Count; i++)
                {
                    List<int> helper_train = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper_train.Add(tree.Train_Predictions[i].Label);
                    }
                    int Most_Occured_Label_train = helper_train.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    int ID = DataTree.Forest[1].Train_Predictions[i].Id;
                    FinalPredictions_Train.Add(new Prediction(ID, Most_Occured_Label_train));
                    //This is needed for SVM and LR to get the training data
                    double[] vector = new double[ForestSize];
                    for (int j = 0; j < helper_train.Count; j++)
                    {
                        if (helper_train[j] == 1)
                        {
                            vector[j] = helper_train[j];
                        }
                        else
                        {
                            vector[j] = 0;
                        }
                    }
                    SVM_LR_Train_Data.Add(new Entry(DataTree.data_1[i].Sign, vector));
                }
                for (int i = 0; i < DataTree.Forest[0].Test_Predictions.Count; i++)
                {
                    List<int> helper_test = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper_test.Add(tree.Test_Predictions[i].Label);
                    }
                    int Most_Occured_Label_test = helper_test.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    int ID = DataTree.Forest[1].Test_Predictions[i].Id;
                    FinalPredictions_Test.Add(new Prediction(ID, Most_Occured_Label_test));

                    //This is needed for SVM and LR to get the training data
                    double[] vector = new double[ForestSize];
                    for (int j = 0; j < helper_test.Count; j++)
                    {
                        if (helper_test[j] == 1)
                        {
                            vector[j] = helper_test[j];
                        }
                        else
                        {
                            vector[j] = 0;
                        }
                    }
                    SVM_LR_Test_Data.Add(new Entry(DataTree.data_2[i].Sign, vector));
                }
            for (int i = 0; i < DataTree.Forest[0].Eval_Predictions.Count; i++)
            {
                List<int> helper_eval = new List<int>();
                foreach (var tree in DataTree.Forest) //loops through each Tree
                {
                    helper_eval.Add(tree.Eval_Predictions[i].Label);
                }
                int Most_Occured_Label_eval = helper_eval.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                int ID = DataTree.Forest[1].Eval_Predictions[i].Id;
                FinalPredictions_Eval.Add(new Prediction(ID, Most_Occured_Label_eval));

                //This is needed for SVM and LR to get the training data
                double[] vector = new double[ForestSize];
                for (int j = 0; j < helper_eval.Count; j++)
                {
                    if (helper_eval[j] == 1)
                    {
                        vector[j] = helper_eval[j];
                    }
                    else
                    {
                        vector[j] = 0;
                    }
                }
                SVM_LR_Eval_Data.Add(new Entry(DataTree.data_3[i].Sign, vector));
            }
            int mm = 0;
            int correct_labels2 = 0;
            foreach (var item in FinalPredictions_Train)
            {
                if (item.Label == DataTree.data_1[mm].Sign)
                {
                    correct_labels2++;
                }
            }
            Console.WriteLine("Training Set Accuracy for Bagged Forest is: " + (Convert.ToDouble(correct_labels2) / Convert.ToDouble(DataTree.data_1.Count)));
            int zz = 0;
            int correct_labels = 0;
            foreach (var item in FinalPredictions_Test)
            {
                if (item.Label == DataTree.data_2[zz].Sign)
                {
                    correct_labels++;
                }
            }
            Console.WriteLine("Test Set Accuracy for Bagged Forest is: " + (Convert.ToDouble(correct_labels) / Convert.ToDouble(DataTree.data_2.Count)));
            //BaggedForest BestTree = DataTree.Forest.OrderByDescending(x => x.Accuracy).First();
            // public Data(Random rand, StreamReader r, StreamReader r2, StreamReader eval, StreamReader eval_ID, int depth)
            //foreach (var item in data8.Accuracies)
            //{
            //    Console.WriteLine(item);
            //}
            //GenerateCSV(data.Predictions, @"\Decision_Tree_1.csv");
            //GenerateCSV(data.Predictions2, @"\Decision_Tree_2.csv");
            //GenerateCSV(data.Predictions3, @"\Decision_Tree_3.csv");
            //GenerateCSV(data.Predictions4, @"\Decision_Tree_4.csv");
            //GenerateCSV(data.Predictions5, @"\Decision_Tree_5.csv");
            //GenerateCSV(data.Predictions_Average, @"\Decision_Tree_Average.csv");
            //

            GenerateCSV(FinalPredictions_Train, "Bagged_Forest.csv");
            //string to_file = "";
            //foreach (var item in SVM_LR_Train_Data)
            //{
            //    to_file += item.Sign + " ";
            //    int i = 1;
            //    foreach (var item2 in item.Vector)
            //    {
            //        to_file += i + ":" + item2 + " ";
            //        i++;
            //    }
            //    to_file += Environment.NewLine;
            //}
            //File.WriteAllText(startupPath + @"\tree.train.data", to_file);

            //to_file = "";
            //foreach (var item in SVM_LR_Test_Data)
            //{
            //    to_file += item.Sign + " ";
            //    int i = 1;
            //    foreach (var item2 in item.Vector)
            //    {
            //        to_file += i + ":" + item2 + " ";
            //        i++;
            //    }
            //    to_file += Environment.NewLine;
            //}
            //File.WriteAllText(startupPath + @"\tree.test.data", to_file);

            //to_file = "";
            //foreach (var item in SVM_LR_Eval_Data)
            //{
            //    to_file += item.Sign + " ";
            //    int i = 1;
            //    foreach (var item2 in item.Vector)
            //    {
            //        to_file += i + ":" + item2 + " ";
            //        i++;
            //    }
            //    to_file += Environment.NewLine;
            //}
            //File.WriteAllText(startupPath + @"\tree.eval.data", to_file);

            #region SVM
            #region Datas
            data1 = new Data(10, 0.00001, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data2 = new Data(10, 1, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data3 = new Data(10, 0.1, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data4 = new Data(10, 0.01, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data5 = new Data(10, 0.001, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data6 = new Data(10, 0.0001, r, false, 0, false, false, 10, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data7 = new Data(10, 0.00001, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data8 = new Data(10, 1, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data9 = new Data(10, 0.1, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data10 = new Data(10, 0.01, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data11 = new Data(10, 0.001, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data12 = new Data(10, 0.0001, r, false, 0, false, false, 1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data13 = new Data(10, 0.00001, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data14 = new Data(10, 1, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data15 = new Data(10, 0.1, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data16 = new Data(10, 0.01, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data17 = new Data(10, 0.001, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data18 = new Data(10, 0.0001, r, false, 0, false, false, 0.1, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data19 = new Data(10, 0.00001, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data20 = new Data(10, 1, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data21 = new Data(10, 0.1, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data22 = new Data(10, 0.01, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data23 = new Data(10, 0.001, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data24 = new Data(10, 0.0001, r, false, 0, false, false, 0.01, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data25 = new Data(10, 0.00001, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data26 = new Data(10, 1, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);            
            data27 = new Data(10, 0.1, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
            data28 = new Data(10, 0.01, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data29 = new Data(10, 0.001, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data30 = new Data(10, 0.0001, r, false, 0, false, false, 0.001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data31 = new Data(10, 0.00001, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data32 = new Data(10, 1, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data33 = new Data(10, 0.1, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data34 = new Data(10, 0.01, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data35 = new Data(10, 0.001, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data36 = new Data(10, 0.0001, r, false, 0, false, false, 0.0001, true, 0, false, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                #endregion

                ListOfDatas = new List<Data>()
                {
                    data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
                    data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
                    data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
                };
                LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
                double learning_rate = LargestData.Learning_Rate; //0.0001
                double C = LargestData.C; //0.001
                Console.WriteLine("\n*******SVM*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + learning_rate + "\n\tC:\t" + C);
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
                Data dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, 20, learning_rate, r, false, 0, false, false, C, true, 0, false, ForestSize);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
                Data dataEval = new Data(SVM_LR_Eval_Data, Eval_ID, learning_rate, dataTest.BestWeightBias, 16);
                GenerateCSV(dataEval.Predictions, @"\Trees_SVM.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval.Test_Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve SVM: \n\t");
                foreach (var item in dataTest.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Logistic Regression
                #region Datas
                data1 = new Data(10, 10, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data2 = new Data(10, 1, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data3 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data4 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data5 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data6 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 0.1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data7 = new Data(10, 10, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data8 = new Data(10, 1, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data9 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data10 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data11 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data12 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 1, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data13 = new Data(10, 10, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data14 = new Data(10, 1, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data15 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data16 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data17 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data18 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 10, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data19 = new Data(10, 10, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data20 = new Data(10, 1, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data21 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data22 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data23 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data24 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 100, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data25 = new Data(10, 10, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data26 = new Data(10, 1, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data27 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data28 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data29 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data30 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 1000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);

                data31 = new Data(10, 10, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data32 = new Data(10, 1, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data33 = new Data(10, 0.1, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data34 = new Data(10, 0.01, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data35 = new Data(10, 0.001, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                data36 = new Data(10, 0.0001, r, false, 0, false, false, 0, false, 10000, true, SVM_LR_Train_Data, SVM_LR_Test_Data, ForestSize);
                #endregion

                ListOfDatas = new List<Data>()
                {
                    data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
                    data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
                    data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
                };
                LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
                learning_rate = LargestData.Learning_Rate;
                double Tradeoff = LargestData.Tradeoff;
                Console.WriteLine("\n******* Logistic Regression *******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Learning Rate:\t" + learning_rate + "\n\tTradeoff:\t" + Tradeoff);
                Console.WriteLine("The cross validation accuracy for the best hyperparameter is: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
                dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, 20, learning_rate, r, false, 0, false, false, 0, false, Tradeoff, true, ForestSize);
                Console.WriteLine("The total number of updates for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
                dataEval = new Data(SVM_LR_Eval_Data, Eval_ID, learning_rate, dataTest.BestWeightBias, 16);
                GenerateCSV(dataEval.Predictions, @"\Trees_Logistic_Regression.csv");
                //Console.WriteLine("Eval Set Accuracy: \n\t" + Math.Round(dataEval.Accuracy, 3));
                Console.WriteLine("The following are the accuracies from the learning curve Logistic Regression: \n\t");
                foreach (var item in dataTest.AccuracyWeightB)
                {
                    Console.WriteLine("\t" + Math.Round(item.Value.Accuracy, 3));
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.ReadKey(false);
                #endregion                
            //}

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