using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VideoTransfer.Data;
using VideoTransfer.Helpers;

namespace VideoTransfer.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Skydivers = new ObservableCollection<SkydiverViewModel>(Context.Instance.Skydivers.Select(s => new SkydiverViewModel(s)));
            InitJumpNumber();
            var driveListener = new DriveListener();
            driveListener.DriveAdded += DriveAdded;
            driveListener.StartListening();
            PropertyChanged += OnPropertyChanged;
        }
  
        public ObservableCollection<SkydiverViewModel> Skydivers { get; set; }

        private int _jumpNumber;
        public int JumpNumber
        {
            get { return _jumpNumber; }
            set
            {
                _jumpNumber = value;
                OnPropertyChanged();
            }
        }

        private string _jumpNumberText;
        public string JumpNumberText
        {
            get { return _jumpNumberText; }
            set
            {
                _jumpNumberText = value;
                OnPropertyChanged();
            }
        }

        private void DriveAdded(object sender, DriveInfo driveInfo)
        {
            foreach (var skydiverViewModel in Skydivers)
                skydiverViewModel.CheckDrive(driveInfo);    
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
    }
}