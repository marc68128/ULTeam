using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using VideoTransfer.Helpers;
using VideoTransfer.Model;

namespace VideoTransfer.ViewModel
{
    public class SkydiverViewModel : BaseViewModel
    {
        private Skydiver _skydiver;
        public SkydiverViewModel(Skydiver s)
        {
            _skydiver = s;
            Name = s.Name;
            Image = s.ImageName;

            InitCommands();
        }

        public string Name { get; set; } 
        public string Image { get; set; }
        public ICommand InitializeCommand { get; set; }

        private void InitCommands()
        {
            InitializeCommand = new Command(_ =>
            {
                SkydiverInitializer.Initialize(_skydiver);
            });
        }
    }
}
