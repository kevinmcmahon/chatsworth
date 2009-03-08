using System;
using System.IO;
using System.Xml.Serialization;

namespace Chatsworth.Core.DataAccess
{
    public static class SerializeHelper
    {
        /// <summary>
        /// Serializing of dataobjects
        /// </summary>
        /// <param name="dataObject">Object to serialize</param>
        /// <returns>XML string of the serialized object</returns>
        public static string Serialize(object dataObject)
        {
            // Serialize object
            XmlSerializer ser = new XmlSerializer(dataObject.GetType());

            //an XmlTextWriter writing to the MemoryStream using the speficied Encoding
            StringWriter writer = new StringWriter();

            ser.Serialize(writer, dataObject);
            writer.Close();
            return writer.ToString();
        }

        /// <summary>
        /// returns a dataobject contaning the data in the XML string 
        /// </summary>
        /// <param name="objectType">Type for objectcraetion</param>
        /// <param name="objectXML">XML string</param>
        /// <returns>The deserialized object</returns>
        public static object Deserialize(Type objectType, string objectXML)
        {
            TextReader reader = new StringReader(objectXML);
            XmlSerializer ser = new XmlSerializer(objectType);

            return ser.Deserialize(reader);
        }
    }
}