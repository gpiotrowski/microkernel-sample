using Microkernel.Contract;

namespace Microkernel.TestPlugin
{
    public class ProcessItemPlugin : IProcessItemPlugin
    {
        public long Process(long value)
        {
            return value * 2;
        }
    }
}
