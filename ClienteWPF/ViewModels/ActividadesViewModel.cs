using ClienteWPF.Models.DTOs;
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
        public int StatusId { get; set; }
        public string StatusAct { get; set; }

        public List<Estado> Estados => Enum.GetValues(typeof(Estado)).Cast<Estado>().ToList();
        public ObservableCollection<ActividadDTO> ListaActividades { get; set; } = new();

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
            //Modo = "inicio";
        }

        [RelayCommand]
        public async Task GetActividades(string estado)
        {
            ListaActividades.Clear();

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

                ListaActividades.Add(actividad);
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
