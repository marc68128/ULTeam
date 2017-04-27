using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoTransfer.Data;
using VideoTransfer.Helpers;

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
            get { return _jumpNumber; }
            set
            {
                _jumpNumber = value;
                OnPropertyChanged();
            }
        }
        public string JumpNumberText
        {
            get { return _jumpNumberText; }
            set
            {
                _jumpNumberText = value;
                OnPropertyChanged();
            }
        }
        public bool ShowModal
        {
            get { return _showModal; }
            set
            {
                _showModal = value;
                OnPropertyChanged();
            }
        }
        public string ModalTitle
        {
            get { return _modalTitle; }
            set
            {
                _modalTitle = value;
                OnPropertyChanged();
            }
        }
        public string ModalMessage
        {
            get { return _modalMessage; }
            set
            {
                _modalMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public Command HideModalCommand { get; set; }

        #endregion

        #region Private methods

        private void DriveAdded(object sender, DriveInfo driveInfo)
        {
            var diskFound = false;
            foreach (var skydiverViewModel in Skydivers)
                diskFound = skydiverViewModel.CheckDrive(driveInfo) || diskFound;

            if (!diskFound)
            {
                ModalTitle = "Attention !";
                ModalMessage = "Le disque ajouté n'a pas été initialisé.\nPour l'utiliser, choisisez un profil et initialisez le disque.";
                ShowModal = true;
                Task.Run(() =>
                {
                    Task.Delay(2000);
                    ShowModal = false;
                });
            }
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
        }

        #endregion
    }
}