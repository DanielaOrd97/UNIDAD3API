using ClienteWPF.Models.DTOs;
using ClienteWPF.Repositories;
using ClienteWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ClienteWPF.ViewModels
{
    public enum Estado
    {
        Borrador = 0,
        Publicada = 1,
        Eliminada = 2
    }
    public partial class ActividadesViewModel:ObservableObject
    {
        [ObservableProperty]
        private ActividadDTO actividad;

        [ObservableProperty]
        private Estado selectedEstado;

        [ObservableProperty]
        private string imagenSeleccionada;

        APIService service = new();
        ActividadesRepository repo;
        public int StatusId { get; set; }
        public string StatusAct { get; set; }

        public List<Estado> Estados => Enum.GetValues(typeof(Estado)).Cast<Estado>().ToList();
        public ObservableCollection<ActividadDTO> ListaActividades { get; set; } = new();
        public ObservableCollection<ActividadDTO> ListaActividades1 { get; set; } = new();


        //private ObservableCollection<ActividadDTO> listaActividadesB = new();
        //public ObservableCollection<ActividadDTO> ListaActividadesB
        //{
        //    get => listaActividadesB;
        //    set => SetProperty(ref listaActividadesB, value);
        //}

        //private ObservableCollection<ActividadDTO> listaActividadesP = new();
        //public ObservableCollection<ActividadDTO> ListaActividadesP
        //{
        //    get => listaActividadesP;
        //    set => SetProperty(ref listaActividadesP, value);
        //}




        private string modo;
        public string Modo
        {
            get => modo;
            set => SetProperty(ref modo, value);
        }

        private string error;
        public string Error
        {
            get => error;
            set => SetProperty(ref error, value);
        }
        public MainViewModel Mainviewmodel { get; }

        public ActividadesViewModel(MainViewModel mainviewmodel)
        {
            Mainviewmodel = mainviewmodel;
            Actividad = new();
            service = new();
            repo = new();
            //Modo = "inicio";
        }

        //[RelayCommand]
        //public async Task GetAllActividades()
        //{
        //    //ListaActividades.Clear();
        //    ListaActividadesB.Clear();
        //    ListaActividadesP.Clear();

        //    var response = await service.GetAllActividades();

        //    var nuevasActividadesB = new ObservableCollection<ActividadDTO>();
        //    var nuevasActividadesP = new ObservableCollection<ActividadDTO>();

        //    foreach (var actividad in response)
        //    {
        //        if (actividad.Imagen != null)
        //        {
        //            byte[] imageBytes = Convert.FromBase64String(actividad.Imagen);
        //            using (var stream = new MemoryStream(imageBytes))
        //            {
        //                BitmapImage bitmapImage = new();
        //                bitmapImage.BeginInit();
        //                bitmapImage.StreamSource = stream;
        //                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //                bitmapImage.EndInit();
        //                bitmapImage.Freeze();
        //                actividad.Img = bitmapImage;
        //            }
        //        }
        //        if (actividad.Estado == 0)
        //        {
        //            nuevasActividadesB.Add(actividad);
        //        }
        //        else if(actividad.Estado == 1)
        //        {
        //            nuevasActividadesP.Add(actividad);
        //        }
        //    }
        //    ListaActividadesB = nuevasActividadesB;
        //    ListaActividadesP = nuevasActividadesP;
        //}

        [RelayCommand]
        public async Task GetActividades(string estado)
        {
            if(estado == "Borrador")
            {
                ListaActividades.Clear();
            }
            else
            {
                ListaActividades1.Clear();
            }


            foreach (var actividad in await service.GetActividades(estado))
            {
                if (actividad.Imagen != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(actividad.Imagen);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        BitmapImage bitmapImage = new();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                        actividad.Img = bitmapImage;
                    }
                }
                if(actividad.Estado == 0)
                {
                    ListaActividades.Add(actividad);
                }
                else if(actividad.Estado == 1)
                {
                    ListaActividades1.Add(actividad);
                }
               
            }
        }

        [RelayCommand]
        public void VerAgregarActividad()
        {
            Modo = "agregar";
        }

        [RelayCommand]
        public void RegresarVista()
        {
            Actividad = null;
            Modo = "regresar";
        }

        [RelayCommand]
        public async void AgregarActividad()
        {
            if (Actividad != null)
            {
                Actividad.Imagen = ConvertirBase64(imagenSeleccionada);
                Actividad.Estado = (int)SelectedEstado;
                await service.AgregarActividad(Actividad);

                StatusId = Actividad.Estado;

                switch (StatusId)
                {
                    case 0:
                        StatusAct = "Borrador";
                        break;
                    case 1:
                        StatusAct = "Publicadas";
                        break;
                    case 2:
                        StatusAct = "Eliminadas";
                        break;
                }

                GetActividades(StatusAct);
                Modo = "regresar";
            }
        }


        [RelayCommand]
        public void VerEditar()
        {
            Modo = "editar";
        }



        [RelayCommand]
        public async Task EditarActividad(ActividadDTO dto)
        {
            if (dto != null)
            {
                dto.Estado = (int)SelectedEstado;
                await service.ActualizarActividad(dto);
                StatusId = dto.Estado;

                switch (StatusId)
                {
                    case 0:
                        StatusAct = "Borrador";
                        break;
                    case 1:
                        StatusAct = "Publicadas";
                        break;
                    case 2:
                        StatusAct = "Eliminadas";
                        break;
                }

                GetActividades(StatusAct);
                Modo = "regresar";
            }
        }

        [RelayCommand]
        public async Task EliminarActividad()
        {
            if (Actividad != null)
            {
                await service.EliminarActividad((int)Actividad.Id);

                var act = repo.Get(Actividad.Id);
                StatusId = act.Estado;

                switch (StatusId)
                {
                    case 0:
                        StatusAct = "Borrador";
                        break;
                    case 1:
                        StatusAct = "Publicadas";
                        break;
                    case 2:
                        StatusAct = "Eliminadas";
                        break;
                }

                GetActividades(StatusAct);
                Modo = "regresar";
            }
        }

        [RelayCommand]
        public void VerEliminar()
        {
            Modo = "eliminar";
        }

        private string ConvertirBase64(string? imagenSeleccionada)
        {
            if (imagenSeleccionada != null)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(imagenSeleccionada);
                return Convert.ToBase64String(imageArray);
            }
            return "";
        }



        [RelayCommand]
        public void SeleccionarImagen()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagenSeleccionada = openFileDialog.FileName;
            }
        }
    }
}
