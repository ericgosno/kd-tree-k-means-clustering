using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.InverseDocumentFrequency
{
    public class LogarithmInverseDocumentFrequency : iInverseDocumentFrequency
    {

        public double CalculateInverseDocumentFrequency(int numDocument, int rawDocumentFrequency)
        {
            return Math.Log(Convert.ToDouble(numDocument) / Convert.ToDouble(rawDocumentFrequency));
        }
    }
}
