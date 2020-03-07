namespace Microkernel.Contract
{
    public interface IProcessResultPlugin : IPlugin
    {
        void ProcessResult(long result);
    }
}
