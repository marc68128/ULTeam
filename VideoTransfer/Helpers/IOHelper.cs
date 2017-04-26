using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VideoTransfer.Model;

namespace VideoTransfer.Helpers
{
    public static class IOHelper
    {
        #region Public fields

        public static string TodayPath => $"{DateTime.Today.Year}/{DateTime.Today.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR"))}/{DateTime.Today.Day}";
        public static readonly List<string> VideoExtention = new List<string> { ".mp4", ".MP4" };

        #endregion

        #region Public methods

        public static List<CameraItem> GetAllFilesAndFoldersRecursivly(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, false));
            var folders = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories).Select(p => new CameraItem(p, true));
            return files.Concat(folders).OrderBy(x => x.Path).ToList();
        }

        public static double CompareDirectories(this List<CameraItem> driveContent, List<CameraItem> skydiverContent)
        {
            var driveItemCount = driveContent.Count;
            double similaritiesPercentage = driveContent.Where(driveItem => skydiverContent.Any(skydiveerItem => driveItem.Equals(skydiveerItem))).Sum(driveItem => (double)100 / driveItemCount);
            return similaritiesPercentage;
        }

        public static List<CameraItem> GetAddedCameraItems(List<CameraItem> driveContent, List<CameraItem> skydiverContent)
        {
            var added = driveContent.Except(skydiverContent);
            return added.ToList();
        }
        public static List<CameraItem> GetRemovedCameraItems(List<CameraItem> driveContent, List<CameraItem> skydiverContent)
        {
            var removed = skydiverContent.Except(driveContent);
            return removed.ToList();
        }

        #endregion
    }
}