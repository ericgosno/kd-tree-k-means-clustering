using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Stemmer;

namespace StopWordRemoval
{
    public class StopWordRemoval
    {
        private HashSet<string> stopWord;
        public StopWordRemoval()
        {
            stopWord = new HashSet<string>();
            PorterStemmer stemmer = new PorterStemmer();
            //Read Vocab
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string base_url = @"C:\Users\3nc\documents\visual studio 2010\Projects\DataMining_TA\StopWordRemoval\";
            try
            {
                fileStream = new FileStream(base_url + @"english.stop.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);
                while (true)
                {
                    string line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    line = line.ToLower();
                    line = Regex.Replace(line, "[^a-zA-Z0-9   ]", string.Empty);
                    line = stemmer.stem(line);
                    stopWord.Add(line);
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

        public bool IsStopWord(string words)
        {
            if (stopWord.Contains(words)) return true;
            else return false;
        }
    }
}
