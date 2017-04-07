using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ULTeam.Enums;
using ULTeam.Utils;

namespace ULTeam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<DriveInfo> _drives;
        private string _selectedDrive;
        private Skydiver _selectedSkydiver;

        public MainWindow()
        {
            InitializeComponent();
            _drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToList();
        }

        private void SelectSkydiver(object sender, MouseButtonEventArgs e)
        {
            _selectedSkydiver = (Skydiver)Enum.Parse(typeof(Skydiver), (sender as ProgressBar).Name.Replace("ProgressBar", ""));
            RenaudBorder.Visibility = TomBorder.Visibility = MarcBorder.Visibility = Visibility.Collapsed;
            switch (_selectedSkydiver)
            {
                case Skydiver.Renaud:
                    RenaudBorder.Visibility = Visibility.Visible;
                    break;
                case Skydiver.Tom:
                    TomBorder.Visibility = Visibility.Visible;
                    break;
                case Skydiver.Marc:
                    MarcBorder.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ListenDrive();
        }

        private async void ListenDrive()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToList();
                    if (drives.Count > _drives.Count)
                    {
                        var addedDrive = drives.Except(_drives).First();
                        _drives = drives;
                        DriveAdded(addedDrive);
                        continue;
                    }
                    if (drives.Count < _drives.Count)
                    {
                        _drives = drives;
                    }
                }
            });
        }

        private void DriveAdded(DriveInfo addedDrive)
        {
            var drivePath = addedDrive.RootDirectory.FullName;
            List<string> videoPaths = new List<string>(); //TODO Find vidéos in gopro root folder

            var extPath = $"/{DateTime.Today.Year}/{DateTime.Today.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR"))}/{DateTime.Today.Day}";
            var jumpNumber = Directory.GetDirectories(extPath).Select(d => int.Parse(d.Replace("Saut", ""))).Max() + 1;
            extPath = Path.Combine(extPath, "Saut" + jumpNumber, _selectedSkydiver + ".mp4");

            for (var index = 0; index < videoPaths.Count; index++)
            {
                var videoPath = videoPaths[index];
                CustomFileCopier fileCopier = new CustomFileCopier(videoPath, extPath);
                extPath = Path.Combine(Path.GetDirectoryName(extPath), _selectedSkydiver + (index + 1) + ".mp4");
            }
        }
    }
}
