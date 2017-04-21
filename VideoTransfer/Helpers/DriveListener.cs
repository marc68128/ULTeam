using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTransfer.Helpers
{
    public class DriveListener
    {
        #region Private fields

        private List<DriveInfo> _drives;
        private bool _isListening;

        #endregion

        #region Constructors

        public async void StartListening()
        {
            if (_isListening) return;

            _drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToList();
            _isListening = true;

            await Task.Run(() =>
            {
                while (_isListening)
                {
                    var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToList();
                    if (drives.Count > _drives.Count)
                    {
                        var addedDrive = drives.First(d => _drives.All(d2 => d2.RootDirectory.Name != d.RootDirectory.Name));
                        _drives = drives;
                        DriveAdded?.Invoke(this, addedDrive);
                    }
                    if (drives.Count < _drives.Count)
                    {
                        _drives = drives;
                    }
                }
            });
        }

        #endregion

        #region Public methods

        public void StopListening()
        {
            _isListening = false;
        }

        #endregion

        #region Events

        public event EventHandler<DriveInfo> DriveAdded;

        #endregion
    }
}