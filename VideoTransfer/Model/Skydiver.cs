using System;
using System.Collections.Generic;

namespace VideoTransfer.Model
{
    public class Skydiver
    {
        #region Private fields

        private string _identifierFileName;

        #endregion

        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> CurrentVideos { get; set; }
        public bool IsInitialized { get; set; }
        public string ImageName { get; set; }
        public List<CameraItem> CameraItems { get; set; }
        public string IdentifierFileName
        {
            get => string.IsNullOrWhiteSpace(_identifierFileName) ? _identifierFileName = $"VideoTransfer_{Name}_{new Random().Next(0, 9999)}.donotremove" : _identifierFileName;
            set => _identifierFileName = value;
        }

        #endregion
    }
}