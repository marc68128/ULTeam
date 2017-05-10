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
        #region Private fields

        private HomeViewModel ViewModel => DataContext as HomeViewModel;

        #endregion

        #region Constructors

        public HomePage()
        {
            InitializeComponent();
            var homeVm = new HomeViewModel();
            DataContext = homeVm;
            BindSkydivers();
        }

        #endregion

        #region Private methods

        private void RefreshSkydivers()
        {
            RootGrid.Children.Clear();
            RootGrid.ColumnDefinitions.Clear();
            
            for (var index = 0; index < ViewModel.Skydivers.Count; index++)
            {
                var skydiver = ViewModel.Skydivers[index];
                RootGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                var control = new SkydiverControl(skydiver);
                control.SetValue(Grid.ColumnProperty, index);
                RootGrid.Children.Add(control);
            }
        }

        private void BindSkydivers()
        {

            RefreshSkydivers();
            ViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Skydivers")
                    RefreshSkydivers();
            };
            ViewModel.Skydivers.CollectionChanged += (sender, args) => RefreshSkydivers();
        }

        #endregion
    }
}