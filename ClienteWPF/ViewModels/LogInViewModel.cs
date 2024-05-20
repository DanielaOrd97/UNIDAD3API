using ClienteWPF.Models.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.ViewModels
{
    public partial class LogInViewModel:ObservableObject
    {
        [ObservableProperty]
        private LogInDTO login;


        public MainViewModel Mainviewmodel { get; }

        public LogInViewModel(MainViewModel mainviewm)
        {
            //service = new();
            Login = new LogInDTO();
            Mainviewmodel = mainviewm;
        }

        [RelayCommand]
        public async Task LogInn()
        {

            if (Login != null)
            {
                // var response = await service.LogIn(Login);
                Mainviewmodel.NavegarActividades();
                //Mainviewmodel.Modo = "actividades";
            }
        }
    }
}
