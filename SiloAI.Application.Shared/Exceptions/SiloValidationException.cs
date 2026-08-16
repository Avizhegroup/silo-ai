namespace SiloAI.Application.Shared;
public class SiloValidationException : Exception
{
    public List<string> ErrorMessages { get; set; } = new List<string>();

    public SiloValidationException(List<ValidationResult> results)
    {
        foreach (var error in results)
        {
            ErrorMessages.Add(error.ErrorMessage);
        }
    }
}
