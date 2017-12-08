#set -o nounset
mcs Code/Aggressive_DynamicLearningRate_SVM_LR/*.cs
mv Code/Aggressive_DynamicLearningRate_SVM_LR/AccuracyWB.exe Code/Aggressive_DynamicLearningRate_SVM_LR/Aggressive_DynamicLearningRate_SVM_LR.exe

mcs Code/Bagged_Forest/*.cs

mcs Code/Decision_Tree/*.cs
mv Code/Decision_Tree/Data.exe Code/Decision_Tree/Decision_Tree.exe

echo
mono Code/Aggressive_DynamicLearningRate_SVM_LR/Aggressive_DynamicLearningRate_SVM_LR.exe Data_Files/data.train Data_Files/data.test Data_Files/data.eval.anon Data_Files/data.eval.id
echo
echo "*****************Bagged Forest***********************"
mono Code/Bagged_Forest/BaggedForest.exe Data_Files/data.train Data_Files/data.test Data_Files/data.eval.anon Data_Files/data.train.id Data_Files/data.test.id Data_Files/data.eval.id
echo
echo "*****************Decision Tree***********************"
mono Code/Decision_Tree/Decision_Tree.exe Data_Files/data.train Data_Files/data.test Data_Files/data.eval.anon Data_Files/data.train.id Data_Files/data.test.id Data_Files/data.eval.id
#rm Code/Program.exe
exit 0
