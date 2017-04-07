using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ULTeam.Enums;

namespace ULTeam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Skydiver SelectedSkydiver { get; set; }
        private void SelectSkydiver(object sender, MouseButtonEventArgs e)
        {
            SelectedSkydiver = (Skydiver)Enum.Parse(typeof(Skydiver), (sender as ProgressBar).Name.Replace("ProgressBar", ""));
            RenaudBorder.Visibility = TomBorder.Visibility = MarcBorder.Visibility = Visibility.Collapsed;
            switch (SelectedSkydiver)
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
        }
    }
}
