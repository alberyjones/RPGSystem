using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.DataAccess
{
    public interface IDataLoader
    {
        bool TryLoad<T>(string path, out T item) where T : class;

        bool Save<T>(T item, string path) where T : class;
    }

    public class LocalXmlLoader : IDataLoader
    {
        private const string XmlSuffix = ".xml";
        private string rootFolder;

        public LocalXmlLoader(string rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public bool TryLoad<T>(string path, out T item) where T : class
        {
            return TryLoadFile<T>(Path.Combine(rootFolder, path + XmlSuffix), out item);
        }

        public bool Save<T>(T item, string path) where T : class
        {
            return SaveFile<T>(item, Path.Combine(rootFolder, path + XmlSuffix));
        }

        private static bool TryLoadFile<T>(string file, out T item) where T : class
        {
            item = null;
            try
            {
                if (!String.IsNullOrEmpty(file) && File.Exists(file))
                {
                    using (var fs = File.OpenRead(file))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        item = serializer.Deserialize(fs) as T;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static bool SaveFile<T>(T item, string file) where T : class
        {
            try
            {
                if (item != null)
                {
                    using (var fs = File.OpenWrite(file))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(fs, item);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
