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
        private Skydiver _selectedSkydiver;
        private int _currentJumpNumber;

        private string _todayPath => $"{DateTime.Today.Year}/{DateTime.Today.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR"))}/{DateTime.Today.Day}";


        public MainWindow()
        {
            InitializeComponent();
            _currentJumpNumber = 1;
            if (Directory.Exists(_todayPath) && Directory.GetDirectories(_todayPath).Length > 0)
            {
                _currentJumpNumber = Directory.GetDirectories(_todayPath)
                    .Select(d => int.Parse(new string(Path.GetFileName(d).Where(char.IsDigit).ToArray())))
                    .Max() + 1;
            }
            JumpNumberTextBlock.Text = "Saut " + _currentJumpNumber;
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

            _drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).ToList();
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
                        var addedDrive = drives.First(d => _drives.All(d2 => d2.RootDirectory.Name != d.RootDirectory.Name));
                        _drives = drives;
                        DriveAdded(addedDrive);
                        return;
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
            Dispatcher.Invoke(() =>
            {
                RenaudBorder.Visibility = TomBorder.Visibility = MarcBorder.Visibility = Visibility.Collapsed;
            });
            var selectedSkydiver = _selectedSkydiver;
            var drivePath = addedDrive.RootDirectory.FullName;
            List<string> videoPaths = Directory.GetFiles(Path.Combine(drivePath, "DCIM", "101GOPRO"), "*.MP4").ToList();

            //TODO: CHECK NUMBER OF FILES AND SKIP IF > 4

            Directory.CreateDirectory(_todayPath);
            var extPath = Path.Combine(_todayPath, "Saut" + _currentJumpNumber);
            Directory.CreateDirectory(extPath);

            MultipleFileCopier copier = new MultipleFileCopier(videoPaths.Select((s, i) => new Copy(s, Path.Combine(extPath, _selectedSkydiver + (i > 1 ? "_" + i : "") + ".mp4"))).ToList());
            copier.OnProgressChanged += (double persentage, ref bool cancel) =>
            {
                switch (selectedSkydiver)
                {
                    case Skydiver.Renaud:
                        Dispatcher.Invoke(() => { RenaudProgressBar.Value = persentage; });
                        break;
                    case Skydiver.Tom:
                        Dispatcher.Invoke(() => { TomProgressBar.Value = persentage; });
                        break;
                    case Skydiver.Marc:
                        Dispatcher.Invoke(() => { MarcProgressBar.Value = persentage; });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
            copier.Copy();
        }

        private void NewJump(object sender, RoutedEventArgs e)
        {
            JumpNumberTextBlock.Text = "Saut " + ++_currentJumpNumber;
            Dispatcher.Invoke(() =>
            {
                RenaudBorder.Visibility = TomBorder.Visibility = MarcBorder.Visibility = Visibility.Collapsed;
                RenaudProgressBar.Value = TomProgressBar.Value = MarcProgressBar.Value = 0;
            });
        }
    }
}
