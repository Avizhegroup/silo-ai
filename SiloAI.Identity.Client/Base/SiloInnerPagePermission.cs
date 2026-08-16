namespace SiloAI.Identity.Client.Base;
public class SiloInnerPagePermission
{
    public string Name { get; set; }
    public Dictionary<string, object> AdditionalData { get; set; } = new();
}
