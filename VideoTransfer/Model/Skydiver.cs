using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTransfer.Model
{
    public class Skydiver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> CurrentVideos { get; set; }
        public bool IsInitialized { get; set; }
        public string ImageName { get; set; }
        public List<CameraItem> CameraItems { get; set; }
    }
}
