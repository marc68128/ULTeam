using System.Collections.Generic;

namespace VideoTransfer.Model
{
    public class Skydiver
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> CurrentVideos { get; set; }
        public bool IsInitialized { get; set; }
        public string ImageName { get; set; }
        public List<CameraItem> CameraItems { get; set; }
        public string IdentifierFileName => $"VideoTransfer_{Name}_{Id}.donotremove";

        #endregion
    }
}