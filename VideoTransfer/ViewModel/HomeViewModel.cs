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
            }
        }
    }
}