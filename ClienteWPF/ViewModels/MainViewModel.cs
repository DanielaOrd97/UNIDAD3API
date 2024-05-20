
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {

        private readonly LogInViewModel loginviewmodel;
        private readonly ActividadesViewModel actviewmodel;
        private readonly DepartamentosViewModel deptviewmodel;



        private string modo;
        public string Modo
        {
            get => modo;
            set => SetProperty(ref modo, value);
        }

        //private object viewmodelactual;

        //public object ViewModelAactual
        //{
        //    get { return viewmodelactual; }
        //    set { viewmodelactual = value; }
        //}

        private object viewmodelactual;
        public object ViewModelAactual
        {
            get => viewmodelactual;
            set => SetProperty(ref viewmodelactual, value);
        }
        public MainViewModel()
        {
            loginviewmodel = new LogInViewModel(this);
            actviewmodel = new ActividadesViewModel(this);
            deptviewmodel = new DepartamentosViewModel(this);

            //Modo = "login";
            ViewModelAactual = loginviewmodel;
        }

        //[RelayCommand]
        public void NavegarLogIn()
        {
            ViewModelAactual = loginviewmodel;
        }

        public void NavegarDepartamentos()
        {
            ViewModelAactual = deptviewmodel;
            deptviewmodel.GetAllDeptos();
        }

        public void NavegarActividades()
        {
            //Modo = "actividades";
            ViewModelAactual = actviewmodel;
        }
    }
}
