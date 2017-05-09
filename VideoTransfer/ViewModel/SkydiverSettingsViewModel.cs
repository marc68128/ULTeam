using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Data;
using VideoTransfer.Helpers;
using VideoTransfer.Model;
using VideoTransfer.View;

namespace VideoTransfer.ViewModel
{
    public class SkydiverSettingsViewModel : BaseViewModel
    {
        #region Private fields

        private Skydiver Skydiver => Context.Instance.Skydivers.First(s => s.Id == _skydiverId);
        private readonly int _skydiverId;
        private string _name;
        private string _imagePath;
        private bool _deleteVideos;

        #endregion

        #region Constructors

        public SkydiverSettingsViewModel(int skydiverId)
        {
            _skydiverId = skydiverId;
            Name = Skydiver.Name;
            ImagePath = Skydiver.ImageName;
            DeleteVideos = Skydiver.DeleteVideos;

            InitCommands();
        }

        #endregion

        #region Commands

        public ICommand InitializeCommand { get; private set; }
        public ICommand EditPictureCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }
        public bool DeleteVideos
        {
            get => _deleteVideos;
            set
            {
                _deleteVideos = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private methods

        private void InitCommands()
        {
            EditPictureCommand = new Command(o =>
            {
                var dialog = new CommonOpenFileDialog { IsFolderPicker = false, Filters = { new CommonFileDialogFilter("Images", "*.png,*.PNG,*.jpg,*.JPG") } };
                var result = dialog.ShowDialog();
                ImagePath = result == CommonFileDialogResult.Ok ? dialog.FileNames.First() : ImagePath;
            });

            CancelCommand = new Command(_ =>
            {
                BreadcrumbHelper.GotoPage(new HomePage());
            });

            SaveCommand = new Command(_ =>
            {
                Save();
                BreadcrumbHelper.GotoPage(new HomePage());
            });

            InitializeCommand = new Command(_ =>
            {
                SkydiverInitializer.Initialize(Skydiver);
            });

        }
        private void Save()
        {
            Skydiver.ImageName = ImagePath;
            Skydiver.Name = Name;
            Skydiver.DeleteVideos = DeleteVideos;
            Context.Instance.SaveChanges();
        }

        #endregion
    }
}