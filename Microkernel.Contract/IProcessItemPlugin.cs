namespace Microkernel.Contract
{
    public interface IProcessItemPlugin : IPlugin
    {
        long Process(long value);
    }
}
