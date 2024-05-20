using ClienteWPF.Models.DTOs;
using ClienteWPF.Services;
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
         LogInDTO login;

        [ObservableProperty]
        private bool? esAdmin;

        private string error;
        public string Error
        {
            get => error;
            set => SetProperty(ref error, value);
        }

        APIService service;
        public MainViewModel Mainviewmodel { get; }

        public LogInViewModel(MainViewModel mainviewm)
        {
            service = new();
            Mainviewmodel = mainviewm;
            Login = new LogInDTO();
        }

        [RelayCommand]
        public async Task LogInn()
        {
            if (Login != null)
            {
                var response = await service.LogIn(Login);

                 if(response.Errores == null)
                 {
                    esAdmin = response.EsAdmin;
                    Mainviewmodel.NavegarActividades();
                 }
                else
                {
                    Error = response.Errores;
                }
                
            }
        }
    }
}
