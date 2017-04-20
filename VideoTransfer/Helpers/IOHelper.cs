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
        public static readonly List<string> VideoExtention = new List<string> { ".mp4", ".MP4" };

        public static List<CameraItem> GetAllFilesAndFoldersRecursivly(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, false));
            var folders = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, true));
            return files.Concat(folders).OrderBy(x => x.Path).ToList();
        }

        public static double CompareDirectories(this List<CameraItem> driveContent, List<CameraItem> skydiverContent)
        {
            var driveItemCount = driveContent.Count;
            double similaritiesPercentage = driveContent.Where(driveItem => skydiverContent.Any(driveItem.Equals)).Sum(driveItem => (double)100 / driveItemCount);
            return similaritiesPercentage;
        }

        public static List<CameraItem> GetAddedCameraItems(List<CameraItem> driveContent, List<CameraItem> skydiverContent)
        {
            var added = skydiverContent.Except(driveContent);
            return added.ToList();
        }
    }
}
