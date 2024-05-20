using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.ViewModels
{
    public class ActividadesViewModel
    {
        public MainViewModel Mainviewmodel { get; }

        public ActividadesViewModel(MainViewModel mainviewmodel)
        {
            Mainviewmodel = mainviewmodel;
        }
    }
}
