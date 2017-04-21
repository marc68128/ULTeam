using System.Windows;
using System.Windows.Controls;
using VideoTransfer.UserControl;
using VideoTransfer.ViewModel;

namespace VideoTransfer.View
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        #region Constructors

        public HomePage()
        {
            InitializeComponent();
            var homeVm = new HomeViewModel();
            for (var index = 0; index < homeVm.Skydivers.Count; index++)
            {
                var skydiver = homeVm.Skydivers[index];
                RootGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
                var control = new SkydiverControl(skydiver);
                control.SetValue(Grid.ColumnProperty, index);
                RootGrid.Children.Add(control);
            }
        }

        #endregion
    }
}