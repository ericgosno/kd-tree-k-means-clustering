using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extension
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public static void SerializeObject(string filename, object objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public static object DeSerializeObject(string filename)
        {
            object objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            stream.Position = 0;
            objectToSerialize = (object)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
