using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LARSMetaDataExplorer.Serialization
{
    public class FileSerializer
    {
        public static void SerializeToFile<T>(string filePath,T objects) where T : class
        {
            using (var stream = File.OpenWrite(filePath))
            {
                SerializeToStream(stream, objects);
            }
        }

       

        public static void SerializeCollectionToFile<T>(string filePath, IEnumerable<T> objects) where T : class
        {
            using (var stream = File.OpenWrite(filePath))
            {
                SerializeToStream(stream, objects);
            }
        }

        public static T DeserializeFromFile<T>(string filePath) where T : class
        {
            using (var stream = File.OpenRead(filePath))
            {
                return DeserializeFromStream<T>(stream);
            }
        }

        public static IEnumerable<T> DeserializeCollectionFromFile<T>(string filePath) where T : class
        {
            using (var stream = File.OpenRead(filePath))
            {
                return DeserializeFromStream<IEnumerable<T>>(stream);
            }
        }

        private static void SerializeToStream<T>(Stream stream, T obj) 
        {
            var serializer = JsonSerializer.Create();
            
            using (var writer = new StreamWriter(stream))
            {
                serializer.Serialize(writer, obj);
            }
        }

        private static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = JsonSerializer.Create();
            
            using (var reader = new StreamReader(stream))
            {
                return serializer.Deserialize<T>(new JsonTextReader(reader));
            }
        }
    }
}
