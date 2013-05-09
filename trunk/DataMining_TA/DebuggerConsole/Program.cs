using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace DebuggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Dataset dataset = null;
            dataset = PenDigits_Debug.run();
            //Template_BagOfWord_Debug.run();
            //dataset = ImageSegmentation_Debug.run();
            StandardClusteringTesting.run(dataset,10);
            string hold = Console.ReadLine();
        }
    }
}
