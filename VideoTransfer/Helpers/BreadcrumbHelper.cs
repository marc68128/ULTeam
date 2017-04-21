using System.Collections.Generic;
using System.Windows.Controls;

namespace VideoTransfer.Helpers
{
    public static class BreadcrumbHelper
    {
        #region Private fields

        private static readonly Stack<Page> PageStack = new Stack<Page>();
        private static Page _pageCurrent;

        #endregion

        #region Properties

        public static MainWindow MainWindow { get; set; }

        #endregion

        #region Public methods

        public static void GotoPage(Page page)
        {
            if (_pageCurrent != null)
            {
                PageStack.Push(_pageCurrent);
            }
            SwitchDisplayTo(page);
        }

        public static void GoBack()
        {
            Page page = PageStack.Pop();
            SwitchDisplayTo(page);
        }

        #endregion

        #region Private methods

        private static void SwitchDisplayTo(Page page)
        {
            MainWindow.Content = page;
            _pageCurrent = page;
        }

        #endregion
    }
}