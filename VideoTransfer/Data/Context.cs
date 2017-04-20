using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoTransfer.Model;

namespace VideoTransfer.Data
{
    public class Context
    {
        private static Context _instance;
        public static Context Instance => _instance ?? (_instance = new Context());

        public List<Skydiver> Skydivers { get; set; }

        private Context()
        {
            using (FileStream stream = File.Open(Constants.DbFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamReader reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                Skydivers = JsonConvert.DeserializeObject<List<Skydiver>>(content) ?? new List<Skydiver>();
            }
        }

        public void SaveChanges()
        {
            byte[] data = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(Skydivers));
            using (Stream f = File.Open(Constants.DbFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                f.Write(data, 0, data.Length);
            }
        }
    }
}
