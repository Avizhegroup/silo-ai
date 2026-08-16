namespace SiloAI.Application.Api;

public class InsufficientCreditException : Exception
{
    public InsufficientCreditException()
        : base("اعتبار مشتری به پایان رسیده است.")
    {
    }
}
