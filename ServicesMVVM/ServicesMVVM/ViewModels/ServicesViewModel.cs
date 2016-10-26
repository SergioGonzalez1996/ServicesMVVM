using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using ServicesMVVM.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public IEnumerable<Product> AllProducts { get; }

        public int ServiceId { get; set; }

        public DateTime DateService { get; set; }

        public DateTime DateRegistered { get; set; }

        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public decimal Price { get; set; }

        public decimal Value { get { return Price * (decimal)Quantity; } }

        private double quantity { get; set; }

        public double Quantity
        {
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Quantity"));
                    }
                } 
            }
            get
            {
                return quantity;
            }
        }

        #endregion

        #region Constructors
        public ServicesViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            DateRegistered = DateTime.Today; // Always will be Today
            DateService = DateTime.Today; // The view date picker
            Quantity = 1;
            quantity = 1;
            var list = new List<Product>();
            using (var da = new DataAccess())
            {
                var products = da.GetList<Product>(false).OrderBy(p => p.Description);
                foreach (var product in products)
                {
                    list.Add(new Product { Description = product.Description, ProductId = product.ProductId });
                }
            }
            this.AllProducts = list;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }
        private async void Save()
        {
            try
            {
                if (Quantity < 1 || quantity < 1 )
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
        public void UpdateQuantity(object sender, int value)
        {
            Quantity = value;
        }
        #endregion
    }
}
