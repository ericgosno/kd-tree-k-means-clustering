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
            Dictionary<string, KeyValuePair<int,int>> vocabDict = new Dictionary<string, KeyValuePair<int,int>>();
            List<string> vocab = new List<string>();
            Dictionary<string,Dictionary<string, int>> vocabDoc = new Dictionary<string,Dictionary<string, int>>();

            //Dictionary<string, string> class1 = new Dictionary<string, string>();
            //Dictionary<string, string> class2 = new Dictionary<string, string>();
            Dictionary<string, string> class3 = new Dictionary<string, string>();
            //HashSet<string> listclass1 = new HashSet<string>();
            //HashSet<string> listclass2 = new HashSet<string>();
            HashSet<string> listclass3 = new HashSet<string>();

            string base_url = @"E:\5109100153 - Eric\tc\";

            /* Read per directory */
            char[] separator = new char[1] { '\\' };
            string[] directoryPath = Directory.GetDirectories(base_url + @"20_newsgroups");
            List<String> directoryName = new List<string>();
            foreach (string f in directoryPath)
            {
                string[] splitRes = f.Split(separator);
                directoryName.Add(splitRes[splitRes.Length - 1]);
                Console.WriteLine(directoryName[directoryName.Count-1]);
                string[] filePath = Directory.GetFiles(f);
                string categoryFull = splitRes[splitRes.Length - 1];
                string[] category = categoryFull.Split(new char[1] { '.' });

                /* Process per file */
                foreach (string file in filePath)
                {
                    string[] splitFile = file.Split(separator);

                    string news = categoryFull  + "#" + splitFile[splitFile.Length - 1];
                    titleDoc.Add(news);
                    vocabDoc.Add(news,new Dictionary<string,int>());
                    //class1.Add(news, category[0]);
                    //class2.Add(news, category[0] + "." + category[1]);
                    class3.Add(news, categoryFull);
                    //listclass1.Add(category[0]);
                    //listclass2.Add(category[0] + "." + category[1]);
                    listclass3.Add(categoryFull);
                    
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

                            /* Still in Header? */
                            if (isStillHeader == true && !line.Contains(':'))
                            {
                                isStillHeader = false;
                            }

                            line = line.ToLower();
                            line = Regex.Replace(line, "[^a-zA-Z   ]", string.Empty);
                            char[] separator2 = new char[2] { ' ', '\t' };
                            string[] splitLine = line.Split(separator2);
                            
                            /* skip all header except subject */
                            if(isStillHeader && splitLine[0] != "subject") continue;

                            List<string> listWord = splitLine.ToList();
                            if (isStillHeader) listWord.RemoveAt(0);

                            /* Process each word */
                            foreach (string word in listWord)
                            {
                                if (word.Length <= 1) continue;
                                // Stemming word
                                string stemWord = stemmer.stem(word);
                                // if words == stop word then skip
                                if (stopWord.IsStopWord(stemWord)) continue;
                                if (vocabDict.ContainsKey(stemWord))
                                {
                                    if(!vocabDoc[news].ContainsKey(stemWord))
                                    {
                                        vocabDict[stemWord] = new KeyValuePair<int,int>(vocabDict[stemWord].Key,vocabDict[stemWord].Value + 1);
                                        vocabDoc[news].Add(stemWord,0);
                                    }
                                    vocabDoc[news][stemWord]++;
                                    vocabDict[stemWord] = new KeyValuePair<int,int>(vocabDict[stemWord].Key+1,vocabDict[stemWord].Value);

                                    if (vocabDoc[news][stemWord] == 2 && !vocab.Contains(stemWord)) vocab.Add(stemWord);
                                }
                                else
                                {
                                    vocabDict.Add(stemWord, new KeyValuePair<int,int>(1,1));
                                }
                            }
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

            // Write Vocab file
            vocab.Sort();
            List<string> vocabOutput = new List<string>();
            for (int i = 0; i < vocab.Count; i++)
            {
                vocabOutput.Add(vocab[i] + " " + vocabDict[vocab[i]].Key.ToString() + " " + vocabDict[vocab[i]].Value.ToString());
            }
            System.IO.File.WriteAllLines(base_url + @"vocab.newsgroup.withSubject.txt", vocabOutput);

            vocabOutput.Clear();
            vocabOutput = null;

            // Write Bag of words
            List<String> report = new List<string>();
            
            for (int i = 1; i <= titleDoc.Count; i++)
            {
                for (int j = 1; j <= vocab.Count; j++)
                {
                    if(vocabDoc[titleDoc[i-1]].ContainsKey(vocab[j-1]))
                    report.Add(i+","+j+","+vocabDoc[titleDoc[i-1]][vocab[j-1]]);
                }
            }
            System.IO.File.WriteAllLines(base_url + @"docword.newsgroup.withSubject.txt", report);
            report.Clear();
            report = null;
            vocabDoc.Clear();
            vocabDoc = null;
            vocab.Clear();
            vocab = null;

            // Write document information
            List<string> docContains = new List<string>();
            for (int i = 1; i <= titleDoc.Count; i++)
            {
                docContains.Add(titleDoc[i - 1]);
                //docContains.Add(class1[titleDoc[i - 1]]);
                //docContains.Add(class2[titleDoc[i - 1]]);
                docContains.Add(class3[titleDoc[i - 1]]);
            }
            docContains.Insert(0, "1");
            /*string tmp = "Class1 CATEGORY";
            foreach(string cat in listclass1)
            {
                tmp = tmp + " " + cat;
            }
            docContains.Insert(1, tmp);
            tmp = "Class2 CATEGORY";
            foreach (string cat in listclass2)
            {
                tmp = tmp + " " + cat;
            }
            docContains.Insert(2, tmp); */
            string tmp = "Class-Output CATEGORY";
            foreach (string cat in listclass3)
            {
                tmp = tmp + " " + cat;
            }
            docContains.Insert(1, tmp);
            System.IO.File.WriteAllLines(base_url + @"doc.newsgroup.withSubject.txt", docContains);

            Console.WriteLine("finish!");
            Console.Read();
        }
    }
}
