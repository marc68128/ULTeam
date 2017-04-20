using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Data;
using VideoTransfer.Model;

namespace VideoTransfer.Helpers
{
    public static class SkydiverInitializer
    {
        public static void Initialize(Skydiver s)
        {
            var drive = SelectDrive();
            var cameraItems = IOHelper.GetAllFilesAndFoldersRecursivly(drive);
            Context.Instance.Skydivers.FirstOrDefault(sd => sd.Id == s.Id).CameraItems = cameraItems; 
            Context.Instance.SaveChanges();
        }

        private static string SelectDrive()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileNames.First();
            }
            else
            {
                return null; //TODO : Manage not selected file
            }
        }
    }
}
