using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VideoTransfer.Data;
using VideoTransfer.Helpers;
using VideoTransfer.Model;
using VideoTransfer.View;

namespace VideoTransfer.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private fields

        private int _jumpNumber;
        private string _jumpNumberText;
        private bool _showModal;
        private string _modalTitle;
        private string _modalMessage;

        #endregion

        #region Constructors

        public HomeViewModel()
        {
            InitCommands();
            Skydivers = new ObservableCollection<SkydiverViewModel>(Context.Instance.Skydivers.Select(s => new SkydiverViewModel(s)));
            PropertyChanged += OnPropertyChanged;
            InitJumpNumber();
            var driveListener = new DriveListener();
            driveListener.DriveAdded += DriveAdded;
            driveListener.StartListening();
        }

        #endregion

        #region Properties

        public ObservableCollection<SkydiverViewModel> Skydivers { get; set; }
        public int JumpNumber
        {
            get => _jumpNumber;
            set
            {
                _jumpNumber = value;
                OnPropertyChanged();
                JumpNumberText = $"Saut {_jumpNumber}";
            }
            }
        public string JumpNumberText
        {
            get => _jumpNumberText;
            set
            {
                _jumpNumberText = value;
                OnPropertyChanged();
            }
            }
        public bool ShowModal
        {
            get => _showModal;
            set
            {
                _showModal = value;
                OnPropertyChanged();
            }
            }
        public string ModalTitle
        {
            get => _modalTitle;
            set
            {
                _modalTitle = value;
                OnPropertyChanged();
            }
            }
        public string ModalMessage
        {
            get => _modalMessage;
            set
            {
                _modalMessage = value;
                OnPropertyChanged();
            }
            }

        #endregion

        #region Commands

        public Command AddSkydiverCommand { get; set; }
        public Command HideModalCommand { get; set; }
        public Command NextJumpCommand { get; set; }
        public Command PreviousJumpCommand { get; set; }

        #endregion

        #region Private methods

        private void DriveAdded(object sender, DriveInfo driveInfo)
        {
            var diskFound = false;
            foreach (var skydiverViewModel in Skydivers)
                diskFound = skydiverViewModel.CheckDrive(driveInfo) || diskFound;

            if (diskFound) return;

            ModalTitle = "Attention !";
            ModalMessage = "Le disque ajouté n'a pas été initialisé.\nPour l'utiliser, choisisez un profil et initialisez le disque.";
            ShowModal = true;
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "JumpNumber")
            {
                foreach (var skydiverViewModel in Skydivers)
                    skydiverViewModel.JumpNumber = JumpNumber;

                JumpNumberText = "Saut " + JumpNumber;
            }
        }
        private void InitJumpNumber()
        {
            JumpNumber = 1;
            if (Directory.Exists(IOHelper.TodayPath) && Directory.GetDirectories(IOHelper.TodayPath).Length > 0)
            {
                JumpNumber = Directory.GetDirectories(IOHelper.TodayPath)
                                 .Select(d => int.Parse(new string(Path.GetFileName(d).Where(char.IsDigit).ToArray())))
                                 .Max() + 1;
            }
        }
        private void InitCommands()
        {
            HideModalCommand = new Command(o => ShowModal = false);
            NextJumpCommand = new Command(o => JumpNumber++);
            PreviousJumpCommand = new Command(o => JumpNumber--);
            AddSkydiverCommand = new Command(_ =>
            {
                var skydiver = new Skydiver { Id = Context.Instance.LastSkydiverId + 1 };
                Context.Instance.Skydivers.Add(skydiver);
                Context.Instance.SaveChanges();
                Skydivers.Add(new SkydiverViewModel(skydiver));
            });
        }

        #endregion
    }
}