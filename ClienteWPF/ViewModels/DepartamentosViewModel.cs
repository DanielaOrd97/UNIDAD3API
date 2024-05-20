using ClienteWPF.Models.DTOs;
using ClienteWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWPF.ViewModels
{
    public partial class DepartamentosViewModel : ObservableObject
    {

        [ObservableProperty]
        private DepartamentoDTO departamento;
        public MainViewModel Mainviewmodel { get; }

        APIService service;

        private string modo;
        public string Modo
        {
            get => modo;
            set => SetProperty(ref modo, value);
        }

        public ObservableCollection<DepartamentoDTO> ListaDepartamentos{ get; set; } = new();
        //public ObservableCollection<string> ListaDAgregar { get; set; } = new();
       



        public DepartamentosViewModel(MainViewModel mainviewm)
        {
            Mainviewmodel = mainviewm;
            service = new();
            // GetAllDeptos();
            Departamento = new();
        }

        [RelayCommand]
        public void IrAActividades()
        {
            Modo = "act";
            Mainviewmodel.NavegarActividades();
        }

        [RelayCommand]
        public void VerAgregarDepto()
        {
            Modo = "agregardepto";
        }

        [RelayCommand]
        public void RegresarVista()
        {
            Departamento = null;
            Modo = "regresar";
        }

        [RelayCommand]
        public async Task GetAllDeptos()
        {
            ListaDepartamentos.Clear();
            //ListaDAgregar.Clear();

            var response = await service.GetAllDepartamentos();

            foreach (var dept in response)
            {
                ListaDepartamentos.Add(dept);
            }
            ListaDepartamentos.Add(new DepartamentoDTO
            {
                NombreDepartamento = "Ninguno",
                Username = null,
                Password = null,
                IdSuperior = null
            });
        }

        // void Llenarminilista()
        //{
        //    foreach (var item in ListaDepartamentos)
        //    {
        //        var nombreD = item.NombreDepartamento;

        //        ListaDAgregar.Add(nombreD);
        //    }
        //}


        [RelayCommand]
        public async void AgregarDepartamento()
        {
            if(Departamento != null)
            {
                Departamento.IdSuperior = Departamento.Id;
                await service.AgregarDepartamento(Departamento);
            }
            Modo = "regresar";
            GetAllDeptos();
        }
    }
}
