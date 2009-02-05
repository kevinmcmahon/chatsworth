using System.Xml;

namespace ChatsworthLib.Entity
{
    public interface IMapGenerator
    {
        string FileName { get; }
        XmlDocument Generate();
    }
}