namespace SiloAI.Application.Shared;
public class MethodExecutionFailedException : ApplicationException
{
    public MethodExecutionFailedException() : base("در انجام عملیات مشکلی به وجود آمده است")
    {
    }
}
