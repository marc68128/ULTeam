using System.IO;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Data;
using VideoTransfer.Model;

namespace VideoTransfer.Helpers
{
    public static class SkydiverInitializer
    {
        #region Public methods

        public static void Initialize(Skydiver s)
        {
            var drive = SelectDrive();
            if (drive == null) return;
            Initialize(drive, s);
        }

        public static void Initialize(string drivePath, Skydiver s)
        {
            var cameraItems = IOHelper.GetAllFilesAndFoldersRecursivly(drivePath);
            Context.Instance.Skydivers.Single(sd => sd.Id == s.Id).CameraItems = cameraItems;
            Context.Instance.SaveChanges();
            AddDriveIdentifier(drivePath, s);
        }

        #endregion

        #region Private methods

        private static string SelectDrive()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            return result == CommonFileDialogResult.Ok ? dialog.FileNames.First() : null;
        }

        private static void AddDriveIdentifier(string drive, Skydiver s)
        {
            var fileName = Path.Combine(drive, s.IdentifierFileName);
            using (StreamWriter outputFile = new StreamWriter(fileName, true))
                outputFile.WriteLine(Constants.DoNotRemoveMessage);
        }

        #endregion
    }
}