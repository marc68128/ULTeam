using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using VideoTransfer.Data;
using VideoTransfer.Helpers;
using VideoTransfer.Model;
using VideoTransfer.View;

namespace VideoTransfer.ViewModel
{
    public class SkydiverViewModel : BaseViewModel
    {
        #region Private fields

        private double _currentUploadPercentage;
        private Skydiver Skydiver => Context.Instance.Skydivers.Single(s => s.Id == Id);

        #endregion

        #region Constructors

        public SkydiverViewModel(Skydiver s)
        {
            Id = s.Id;
            Name = s.Name;
            Image = s.ImageName;

            InitCommands();
        }

        #endregion

        #region Properties

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public int JumpNumber { get; set; }
        public double CurrentUploadPercentage
        {
            get => _currentUploadPercentage;
            set
            {
                _currentUploadPercentage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand NavigateCommand { get; private set; }

        #endregion

        #region Public methods

        public bool CheckDrive(DriveInfo d)
        {
            if (Skydiver.CameraItems == null) return false;

            var driveContent = IOHelper.GetAllFilesAndFoldersRecursivly(d.RootDirectory.Name);
            var isSkydiverCam =
                Skydiver.CameraItems.CompareDirectories(driveContent) > 80 ||
                File.Exists(Path.Combine(d.RootDirectory.Name, Skydiver.IdentifierFileName));

            if (!isSkydiverCam) return false;

            var addedItems = IOHelper.GetAddedCameraItems(driveContent, Skydiver.CameraItems);
            var removedItems = IOHelper.GetRemovedCameraItems(driveContent, Skydiver.CameraItems);

            foreach (var removedItem in removedItems) Skydiver.CameraItems.Remove(Skydiver.CameraItems.First(ci => ci.Path == removedItem.Path));

            if (!addedItems.Any())
                return true;

            var videos = addedItems.Where(a => !a.IsDirectory && IOHelper.VideoExtention.Any(ext => a.Path.Contains(ext))).Select(i => i.Path);
            var videosArray = videos as string[] ?? videos.ToArray();

            if (!videosArray.Any())
                return true;

            //Create destination directory
            Directory.CreateDirectory(IOHelper.TodayPath);
            var extPath = Path.Combine(IOHelper.TodayPath, "Saut " + JumpNumber);
            Directory.CreateDirectory(extPath);

            //Copy videos;
            MultipleFileCopier copier = new MultipleFileCopier(videosArray.ToList().Select(s => new Copy(s, Path.Combine(extPath, Name + " (" + DateTime.Today.ToString("MM", CultureInfo.GetCultureInfo("fr-FR")) + "-" + DateTime.Today.Day + ") (Saut" + JumpNumber + ").mp4"))).ToList());
            copier.OnProgressChanged += (double persentage, ref bool cancel) =>
            {
                CurrentUploadPercentage = persentage;
            };
            copier.OnComplete += () =>
            {
                CurrentUploadPercentage = 0;
                //Update Db with new files and folders
                Skydiver.CameraItems.AddRange(addedItems);
                Context.Instance.SaveChanges();
            };
            copier.Copy();
            return true;
        }

        #endregion

        #region Private methods

        private void InitCommands()
        {
            NavigateCommand = new Command(_ =>
              {
                  BreadcrumbHelper.GotoPage(new SkydiverSettingsPage(Id));
              });
        }

        #endregion
    }
}