using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using ServicesMVVM.Services;
using System;
using System.Windows.Input;
using System.ComponentModel;

namespace ServicesMVVM.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        //public ProductsViewModel EditProduct { get; private set; }

        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Description))
                {
                    await dialogService.ShowMessage("Error", "Debes ingresar una descripción.");
                    return;
                }
                if (Price < 0)
                {
                    await dialogService.ShowMessage("Error", "Debes ingresar un precio positivo.");
                    return;
                }
                using (var daa = new DataAccess())
                {
                    /*
                     // I'm getting a error on Where, I don't know how to fix it, need help
                    var checkDescription = daa.First<Product>(false).Where(p => p.Description == Description).FirstOrDefault();
                    if (checkDescription != null)
                    {
                        await dialogService.ShowMessage("Error", "Ya hay existe producto con descripción " + Description + ".");
                        return;
                    }
                    */
                }

                var product = new Product
                {
                    ProductId = ProductId,
                    Description = Description,
                    Price = Price,
                };

                using (var da = new DataAccess())
                {
                    da.Insert(product);
                }

                Description = "";
                Price = 0;
                OnPropertyChanged("Description");
                OnPropertyChanged("Price");

                await dialogService.ShowMessage("Información", "El producto ha sido creado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    await dialogService.ShowMessage("Error", "Ya existe un producto con esta descripción");
                }
                else if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    await dialogService.ShowMessage("Error", "Imposible eliminar el producto porque tiene registros relacionados.");
                }
                else
                {
                    await dialogService.ShowMessage("Error", "Ha ocurrido un error inesperado: " + ex.Message);
                }
            }
        }

        public ICommand EditCommand { get { return new RelayCommand(Edit); } }
        private void Edit()
        {

            //await dialogService.ShowMessage("Informacion", "Producto: " + ProductId + " / " + Description + " / " + Price);
            //EditProduct = new EditProductsViewModel(ProductId, Description, Price);
            //EditProduct = new ProductsViewModel();
            //NewProduct = new EditProductsViewModel();
            //EditProduct.ProductId = ProductId;
            //EditProduct.Description = "adasdasd";
            //EditProduct.Price = 1657981;
            //await dialogService.ShowMessage("Error", "Producto: " + EditProducts.ProductId + " / " + EditProducts.Description + " / " + EditProducts.Price);
            //OnPropertyChanged("Description");
            //OnPropertyChanged("Price");

            // Trying to make a Edit Products Page... But it doesn't seem like is going to work.

            navigationService.Navigate("EditProductsPage");
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

    /*
     <Entry
        TextColor="{StaticResource FontColor}"
        Text="{Binding ProductId, Mode=TwoWay}"
        Placeholder="ID del Producto"
        Keyboard="Numeric"
        BackgroundColor="{StaticResource BackgroundColor}"/>
    */
}
