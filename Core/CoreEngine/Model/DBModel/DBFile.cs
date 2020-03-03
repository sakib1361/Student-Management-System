using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace CoreEngine.Model.DBModel
{
    public class DBFile
    {

        public string Id { get; set; }
        /// <summary>
        /// Can be duplicate
        public string FileName { get; set; }
        /// Should be unique
        /// </summary>
        public string FilePath { get; set; }
        public string FileHash { get; set; }
        public string Description { get; set; }
        [NotMapped]
        [JsonIgnore]
        public Stream FileStream { get; private set; }

        public DBFile()
        {

        }

        public DBFile(Stream stream, string filename)
        {
            FileStream = stream;
            FileName = filename;
        }

        public DBFile(string filepath)
        {
            FilePath = filepath;
            FileName = Path.GetFileName(filepath);
            FileStream = File.OpenRead(filepath);
        }
    }
}
