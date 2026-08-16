using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace SiloAI.Shared;
public static partial class TextResourceTools
{
    public static string GetResourcePath(IConfiguration configuration)
    {
        string path;

        path = configuration["ProjectConfigs:WmsConfigs:TextResourcePath"];

        if (path.HasNoValue())
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TextResources.resx");
        }

        return path;
    }

    public static Dictionary<string, string> GetTextResourceList(IConfiguration configuration)
    {
        var xmlDocument = XDocument.Load(TextResourceTools.GetResourcePath(configuration));

        var keyValuePairs = xmlDocument.Descendants("data")
            .ToDictionary(
                element => element.Attribute("name")?.Value,
                element => element.Element("value")?.Value
            );

        return keyValuePairs;
    }

    public static void SaveDictionaryToXml(Dictionary<string, string> keyValuePairs, IConfiguration configuration)
    {
        var xmlDocument = new XDocument(new XElement("root",
            keyValuePairs.Select(kv => new XElement("data", new XAttribute("name", kv.Key),
                                                   new XElement("value", kv.Value)))));

        xmlDocument.Save(TextResourceTools.GetResourcePath(configuration));
    }
}

