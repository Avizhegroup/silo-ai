namespace SiloAI.Application.Shared;
public class ProductNotFoundException : Exception
{
    public List<string> Errors;
    public ProductNotFoundException(List<string> errors)
          : base("کدکالا یافت نشد")
    {
        Errors = errors;

        Errors.Add("کدکالا یافت نشد");
    }
}
