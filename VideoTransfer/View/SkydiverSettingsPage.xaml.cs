using System.Windows.Controls;
using VideoTransfer.ViewModel;

namespace VideoTransfer.View
{
    /// <summary>
    /// Interaction logic for SkydiverSettingsPage.xaml
    /// </summary>
    public partial class SkydiverSettingsPage : Page
    {
        public SkydiverSettingsPage(int skydiverId)
        {
            InitializeComponent();
            DataContext = new SkydiverSettingsViewModel(skydiverId);
        }
    }
}