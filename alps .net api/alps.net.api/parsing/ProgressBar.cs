using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.parsing
{
    class ProgressBar
    {
        private int currentProgress;
        private int maxProgress;
        private const string progressFill = "###";
        private const string restFill = "   ";

        public ProgressBar(int maxProgress)
        {
            currentProgress = 0;
            this.maxProgress = maxProgress;
            Console.WriteLine("Progress: ");
            Console.WriteLine("[" + getFill() + "]");
        }

        public void increaseProgress()
        {
            currentProgress++;
            Console.WriteLine("[" + getFill() + "]");
        }

        private string getFill()
        {
            string fill = "";
            for (int i = 0; i < currentProgress; i++) { fill += progressFill; }
            for (int i = currentProgress; i < maxProgress; i++) { fill += restFill; }
            return fill;
        }
    }
}
