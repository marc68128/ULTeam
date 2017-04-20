using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Data;
using VideoTransfer.Helpers;
using VideoTransfer.Model;

namespace VideoTransfer.ViewModel
{
    public class SkydiverViewModel : BaseViewModel
    {
        private string _todayPath => $"{DateTime.Today.Year}/{DateTime.Today.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR"))}/{DateTime.Today.Day}";
        private Skydiver _skydiver => Context.Instance.Skydivers.Single(s => s.Id == Id);
        public SkydiverViewModel(Skydiver s)
        {
            Id = s.Id;
            Name = s.Name;
            Image = s.ImageName;

            InitCommands();
        }

        public int Id { get; private set; }
        public string Name { get; private set; } 
        public string Image { get; private set; }
        public int JumpNumber { get; set; }
        public double CurrentUploadPercentage { get; set; }
        public ICommand InitializeCommand { get; private set; }


        public void CheckDrive(DriveInfo d)
        {
            var driveContent = IOHelper.GetAllFilesAndFoldersRecursivly(d.RootDirectory.Name);
            var isSkydiverCam =  _skydiver.CameraItems.CompareDirectories(driveContent) > 80;

            if (!isSkydiverCam) return; 
 
            
            var addedItems = IOHelper.GetAddedCameraItems(driveContent, _skydiver.CameraItems);

            if (!addedItems.Any())
                return;

            var videos = addedItems.Where(a => !a.IsDirectory && IOHelper.VideoExtention.Any(ext => a.Path.Contains(ext))).Select(i => i.Path);
            var videosArray = videos as string[] ?? videos.ToArray();
            if (videosArray.Any())
            {
                //TODO: Create destination directory
                Directory.CreateDirectory(_todayPath);
                var extPath = Path.Combine(_todayPath, "Saut" + JumpNumber);
                Directory.CreateDirectory(extPath);

                //TODO: Copy videos;
                MultipleFileCopier copier = new MultipleFileCopier(videosArray.Select((s, i) => new Copy(s, Path.Combine(extPath, Name + (i > 1 ? "_" + i : "") + ".mp4"))).ToList());
                copier.OnProgressChanged += (double persentage, ref bool cancel) => CurrentUploadPercentage = persentage;
            }

            //TODO: Refresh list  
            _skydiver.CameraItems.AddRange(addedItems);
            Context.Instance.SaveChanges();
        }

        private void InitCommands()
        {
            InitializeCommand = new Command(_ =>
            {
                SkydiverInitializer.Initialize(_skydiver);
            });
        }
    }
}
