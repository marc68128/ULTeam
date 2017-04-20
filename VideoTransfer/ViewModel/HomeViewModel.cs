using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoTransfer.Data;
using VideoTransfer.Model;

namespace VideoTransfer.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Skydivers = new ObservableCollection<SkydiverViewModel>(Context.Instance.Skydivers.Select(s => new SkydiverViewModel(s)));  
        }

        public ObservableCollection<SkydiverViewModel> Skydivers { get; set; }
    }
}
