using Microkernel.Contract;
using System;

namespace Microkernel.TestPlugin
{
    public class ProcessResultPlugin : IProcessResultPlugin
    {
        public void ProcessResult(long result)
        {
            Console.WriteLine($"Result: {result}");
        }
    }
}
