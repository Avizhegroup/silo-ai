namespace SiloAI.Application.Shared;
public class SqlServerConnectionStringException : Exception
{
    public string Key { get; set; }
    public SqlServerConnectionStringException()
    {

    }
    public SqlServerConnectionStringException(string key)
    {
        Key = key;
    }
}
