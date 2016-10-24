using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using ServicesMVVM.Services;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public int ServiceId { get; set; }

        public DateTime DateService { get; set; }

        public DateTime DateRegistered { get; set; }

        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Value { get { return Price * (decimal)Quantity; } }
        #endregion

        #region Constructors
        public ServicesViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            DateRegistered = DateTime.Today; // Always will be Today
            DateService = DateTime.Today; // The view date picker
            OnPropertyChanged("Quantity");
        }
        #endregion

        #region Commands
        public ICommand ValueChangedCommand { get { return new RelayCommand(ValueChanged); } }
        private void ValueChanged()
        {
            Quantity = Quantity + 1;
            OnPropertyChanged("Quantity");
        }

        public ICommand SaveCommand { get { return new RelayCommand(Save); } }
        private async void Save()
        {
            try
            {
                if (Quantity < 1)
                {
                    await dialogService.ShowMessage("Error", "Debes ingresar una cantidad positiva mayor que 0.");
                    return;
                }

                using (var daa = new DataAccess())
                {
                    var thisProduct = daa.Find<Product>(ProductId, false);
                    if (thisProduct == null)
                    {
                        await dialogService.ShowMessage("Error", "El Producto ID " + ProductId + " no existe.");
                        return;
                    }
                }

                var service = new Service
                {
                    ServiceId = ServiceId,
                    DateRegistered = DateRegistered,
                    DateService = DateService,
                    ProductId = ProductId,
                    Price = Price,
                    Quantity = Quantity,
                };

                using (var da = new DataAccess())
                {
                    da.Insert(service);
                }

                await dialogService.ShowMessage("Información", "El servicio ha sido creado");
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", "Ha ocurrido un error inesperado: " + ex.Message);
            }
        }

        public ICommand EditCommand { get { return new RelayCommand(Edit); } }
        private void Edit()
        {
            navigationService.Navigate("EditServicesPage");
        }
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
