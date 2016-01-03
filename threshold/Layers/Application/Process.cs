using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threshold.Layers.Application
{
    public class Process
    {
        public string processId;
        public bool isListening;

        public Process(string pid)
        {
            processId = pid;
        }

    }
}
