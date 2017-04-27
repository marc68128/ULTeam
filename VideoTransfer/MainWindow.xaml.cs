using System.Windows;
using VideoTransfer.Helpers;
using VideoTransfer.View;

namespace VideoTransfer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            BreadcrumbHelper.MainWindow = this;
            BreadcrumbHelper.GotoPage(new HomePage());
        }

        #endregion
    }
}