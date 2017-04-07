using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULTeam.Enums;
using ULTeam.Utils;

namespace ULTeam
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            SelectSkydiver = new Command(o =>
            {
                if (typeof(Skydiver) != o.GetType())
                    throw new ArgumentException("SelectSkydiver use a skydiver as parameter");

                SelectedSkydiver = (Skydiver)Enum.Parse(typeof(Skydiver), o.ToString());
            });
        }

        public Skydiver SelectedSkydiver { get; set; }
        public Command SelectSkydiver { get; set; }
    }
}
