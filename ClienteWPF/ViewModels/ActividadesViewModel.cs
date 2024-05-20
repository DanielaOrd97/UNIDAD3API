using ClienteWPF.Models.DTOs;
using ClienteWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        APIService service = new();
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

    }
}
