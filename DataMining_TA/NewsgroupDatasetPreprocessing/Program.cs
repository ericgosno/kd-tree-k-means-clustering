using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Stemmer;
using StopWordRemoval;

namespace NewsgroupDatasetPreprocessing
{
    class Program
    {
        static void Main(string[] args)
        {
            PorterStemmer stemmer = new PorterStemmer();

            StopWordRemoval.StopWordRemoval stopWord = new StopWordRemoval.StopWordRemoval();
            List<string> titleDoc = new List<string>();
            //HashSet<string> vocabList = new HashSet<string>();
            Dictionary<string, KeyValuePair<int,int>> vocabDict = new Dictionary<string, KeyValuePair<int,int>>();
            List<string> vocab = new List<string>();
            Dictionary<string,Dictionary<string, int>> vocabDoc = new Dictionary<string,Dictionary<string, int>>();            
            string base_url = @"D:\tc\";

            char[] separator = new char[1] { '\\' };
            string[] directoryPath = Directory.GetDirectories(base_url + @"20_newsgroups");
            List<String> directoryName = new List<string>();
            foreach (string f in directoryPath)
            {
                string[] splitRes = f.Split(separator);
                directoryName.Add(splitRes[splitRes.Length - 1]);
                Console.WriteLine(directoryName[directoryName.Count-1]);
                string[] filePath = Directory.GetFiles(f);
                foreach (string file in filePath)
                {
                    //Console.WriteLine(file);
                    string[] splitFile = file.Split(separator);
                    string news = splitRes[splitRes.Length - 1] + "#" + splitFile[splitFile.Length - 1];
                    titleDoc.Add(news);
                    vocabDoc.Add(news,new Dictionary<string,int>());
                    // Read File
                    
                    FileStream fileStream = null;
                    StreamReader streamReader = null;
                    try
                    {
                        fileStream = new FileStream(file, FileMode.Open);
                        streamReader = new StreamReader(fileStream);
                        bool isStillHeader = true;
                        while (true)
                        {
                            string line = streamReader.ReadLine();
                            if (line == null) break;
                            if (string.IsNullOrEmpty(line))
                                continue;

                            if (isStillHeader == true && !line.Contains(':')) isStillHeader = false;
                            if (isStillHeader) continue;

                            line = line.ToLower();
                            line = Regex.Replace(line, "[^a-zA-Z   ]", string.Empty);

                            char[] separator2 = new char[2] { ' ','\t' };
                            string[] splitLine = line.Split(separator2);
                            foreach (string word in splitLine)
                            {
                                if (word.Length <= 1) continue;
                                string stemWord = stemmer.stem(word);
                                if (stopWord.IsStopWord(stemWord)) continue;
                                if (vocabDict.ContainsKey(stemWord))
                                {
                                    if(!vocabDoc[news].ContainsKey(stemWord))
                                    {
                                        vocabDict[stemWord] = new KeyValuePair<int,int>(vocabDict[stemWord].Key,vocabDict[stemWord].Value + 1);
                                        vocabDoc[news].Add(stemWord,0);
                                        if (vocabDict[stemWord].Value == 2) vocab.Add(stemWord);
                                    }
                                    vocabDoc[news][stemWord]++;
                                    vocabDict[stemWord] = new KeyValuePair<int,int>(vocabDict[stemWord].Key+1,vocabDict[stemWord].Value);
                                    
                                }
                                else
                                {
                                    vocabDict.Add(stemWord, new KeyValuePair<int,int>(1,1));
                                }
                                //if (vocabList.Add(stemWord)) vocab.Add(stemWord); 
                                //Console.Write(stemWord + " ");
                            }
                            //Console.Write("\n");
                        }
                    }
                    finally
                    {
                        if (streamReader != null)
                            streamReader.Close();
                        if (fileStream != null)
                            fileStream.Close();
                    }
                    
                }
                Console.WriteLine("Vocab Now : " + vocab.Count.ToString());
            }

            vocab.Sort();
            System.IO.File.WriteAllLines(base_url + @"vocab_newsgroup.txt", vocab);
            List<String> report = new List<string>();
            for (int i = 1; i <= titleDoc.Count; i++)
            {
                for (int j = 1; j <= vocab.Count; j++)
                {
                    if(vocabDoc[titleDoc[i-1]].ContainsKey(vocab[j-1]))
                    report.Add(i+","+j+","+vocabDoc[titleDoc[i-1]][vocab[j-1]]);
                }
            }
            System.IO.File.WriteAllLines(base_url + @"doc_vocab_newsgroup.txt", report);
            titleDoc.Insert(0, "0");
            System.IO.File.WriteAllLines(base_url + @"docs_newsgroup.txt", titleDoc);

            Console.WriteLine("finish!");
            Console.Read();
            //DirectoryInfo di = new DirectoryInfo(@"D:\tc\20_newsgroups");
        }
    }
}
