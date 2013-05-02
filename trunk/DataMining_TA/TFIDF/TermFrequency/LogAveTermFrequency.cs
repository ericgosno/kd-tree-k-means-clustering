using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public class LogAveTermFrequency : iTermFrequency
    {
        double averageRawTermFrequency;
        public LogAveTermFrequency(double averageRawTermFrequency)
        {
            this.averageRawTermFrequency = averageRawTermFrequency;
        }


        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return (1.0 + Math.Log(Convert.ToDouble(RawTermFrequency))) / (1.0 + Math.Log(averageRawTermFrequency));
        }
    }
}
