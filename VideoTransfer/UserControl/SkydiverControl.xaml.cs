using VideoTransfer.ViewModel;

namespace VideoTransfer.UserControl
{
    /// <summary>
    /// Logique d'interaction pour SkydiverControl.xaml
    /// </summary>
    public partial class SkydiverControl 
    {
        #region Constructor

        public SkydiverControl(SkydiverViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        #endregion
    }
}