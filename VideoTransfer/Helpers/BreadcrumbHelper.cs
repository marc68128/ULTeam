using System.Collections.Generic;
using System.Windows.Controls;

namespace VideoTransfer.Helpers
{
    public static class BreadcrumbHelper
    {
        private static readonly Stack<Page> PageStack = new Stack<Page>();
        private static Page _pageCurrent;
        public static MainWindow MainWindow { get; set; }

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

        private static void SwitchDisplayTo(Page page)
        {
            MainWindow.Content = page;
            _pageCurrent = page;
        }
    }
}