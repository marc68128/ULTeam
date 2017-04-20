using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoTransfer.Model;

namespace VideoTransfer.Helpers
{
    public static class IOHelper
    {
        public static List<CameraItem> GetAllFilesAndFoldersRecursivly(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, false));
            var folders = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, true));
            return files.Concat(folders).OrderBy(x => x.Path).ToList();
        }
    }
}
