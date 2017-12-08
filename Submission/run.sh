#set -o nounset
mcs Code/Aggressive_DynamicLearningRate_SVM_LR/*.cs
mv Code/Aggressive_DynamicLearningRate_SVM_LR/AccuracyWB.exe Code/Aggressive_DynamicLearningRate_SVM_LR/Aggressive_DynamicLearningRate_SVM_LR.exe

mcs Code/Bagged_Forest/*.cs

mcs Code/Decision_Tree/*.cs
mv Code/Decision_Tree/Data.exe Code/Decision_Tree/Decision_Tree.exe

mono Code/Aggressive_DynamicLearningRate_SVM_LR/Aggressive_DynamicLearningRate_SVM_LR.exe Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.train Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.test Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.anon Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.id
mono Code/Bagged_Forest/BaggedForest.exe Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.train Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.test Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.anon Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.id
mono Code/Decision_Tree/Decision_Tree.exe Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.train Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.test Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.anon Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.train.id Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.test.id Data_Files\(Empty,\ if\ need\ place\ all\ of\ them\ here\)/data.eval.id
#rm Code/Program.exe
exit 0
