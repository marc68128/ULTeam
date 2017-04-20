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
using VideoTransfer.ViewModel;

namespace VideoTransfer.UserControl
{
    /// <summary>
    /// Logique d'interaction pour SkydiverControl.xaml
    /// </summary>
    public partial class SkydiverControl 
    {
        public SkydiverControl(SkydiverViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
