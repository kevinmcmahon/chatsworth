using System.Xml;

namespace Chatsworth.Core.Entity
{
    public interface IMapGenerator
    {
        string FileName { get; }
        XmlDocument Generate();
    }
}