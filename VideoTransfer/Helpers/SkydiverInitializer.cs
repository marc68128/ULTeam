using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Data;
using VideoTransfer.Model;

namespace VideoTransfer.Helpers
{
    public static class SkydiverInitializer
    {
        #region Constructors

        public static void Initialize(Skydiver s)
        {
            var drive = SelectDrive();
            if (drive == null) return;
            var cameraItems = IOHelper.GetAllFilesAndFoldersRecursivly(drive);
            Context.Instance.Skydivers.Single(sd => sd.Id == s.Id).CameraItems = cameraItems;
            Context.Instance.SaveChanges();
        }

        #endregion

        #region Private methods

        private static string SelectDrive()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileNames.First();
            }
            return null; 
        }

        #endregion
    }
}