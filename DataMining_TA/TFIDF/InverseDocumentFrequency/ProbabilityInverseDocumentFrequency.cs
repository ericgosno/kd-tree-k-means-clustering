using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.InverseDocumentFrequency
{
    public class ProbabilityInverseDocumentFrequency : iInverseDocumentFrequency
    {

        public double CalculateInverseDocumentFrequency(int numDocument, int rawDocumentFrequency)
        {
            return Math.Max(0.0, Math.Log(Convert.ToDouble(numDocument - rawDocumentFrequency) / Convert.ToDouble(rawDocumentFrequency)));
        }
    }
}
