using System.IO;
using System.Xml.Serialization;

namespace _2C2P.FileUploader.Helper.Serializers
{
    public class XmlSerializerHelper
    {
        /// <summary>
        /// StreamDeserialize xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T StreamDeserialize<T>(Stream stream) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var result = (T)serializer.Deserialize(stream);
            return result;
        }
    }
}
