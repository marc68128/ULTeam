using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using VideoTransfer.Data;
using VideoTransfer.Helpers;
using VideoTransfer.Model;

namespace VideoTransfer.ViewModel
{
    public class SkydiverViewModel : BaseViewModel
    {
        #region Private fields

        private double m_CurrentUploadPercentage;
        private Skydiver _skydiver => Context.Instance.Skydivers.Single(s => s.Id == Id);

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
            get { return m_CurrentUploadPercentage; }
            set
            {
                m_CurrentUploadPercentage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand InitializeCommand { get; private set; }

        #endregion

        #region Public methods

        public void CheckDrive(DriveInfo d)
        {
            if (_skydiver.CameraItems == null) return; 

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
                //Create destination directory
                Directory.CreateDirectory(IOHelper.TodayPath);
                var extPath = Path.Combine(IOHelper.TodayPath, "Saut " + JumpNumber);
                Directory.CreateDirectory(extPath);

                //Copy videos;
                MultipleFileCopier copier = new MultipleFileCopier(videosArray.ToList().Select((s, i) => new Copy(s, Path.Combine(extPath, Name + "_" + DateTime.Today.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR")) + "_" + DateTime.Today.Day + "_Saut" + JumpNumber + (i > 1 ? "_" + i : "") + ".mp4"))).ToList());
                copier.OnProgressChanged += (double persentage, ref bool cancel) =>
                {
                    CurrentUploadPercentage = persentage;
                };;
                copier.OnComplete += () =>
                {
                    CurrentUploadPercentage = 0;
                    //Update Db with new files and folders
                    _skydiver.CameraItems.AddRange(addedItems);
                    Context.Instance.SaveChanges();
                };
                copier.Copy();
            }
        }

        #endregion

        #region Private methods

        private void InitCommands()
        {
            InitializeCommand = new Command(_ =>
            {
                SkydiverInitializer.Initialize(_skydiver);
            });
        }

        #endregion
    }
}