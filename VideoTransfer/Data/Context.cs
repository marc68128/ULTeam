﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VideoTransfer.Model;

namespace VideoTransfer.Data
{
    public class Context
    {
        #region Singleton implementation

        private static Context _instance;
        public static Context Instance => _instance ?? (_instance = new Context());

        #endregion

        #region Constructors

        private Context()
        {
            using (FileStream stream = File.Open(Constants.DbFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamReader reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                Skydivers = JsonConvert.DeserializeObject<List<Skydiver>>(content) ?? new List<Skydiver>();
            }
        }

        #endregion

        #region Properties

        public List<Skydiver> Skydivers { get; set; }

        public int LastSkydiverId => Skydivers == null || !Skydivers.Any() ? -1 : Skydivers.Max(s => s.Id);

        #endregion

        #region Public methods

        public void SaveChanges()
        {
            File.WriteAllText(Constants.DbFilePath, string.Empty);
            byte[] data = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(Skydivers, Formatting.Indented));
            using (Stream f = File.Open(Constants.DbFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                f.Write(data, 0, data.Length);
            }
        }

        #endregion
    }
}