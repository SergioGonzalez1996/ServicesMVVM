using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using ServicesMVVM.Services;
using System;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    public class ProductsViewModel
    {
        #region Attributes
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Properties
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


                await dialogService.ShowMessage("Información", "El producto ha sido creado");
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", "Ha ocurrido un error inesperado: " + ex.Message);
            }
        }

        public ICommand EditCommand { get { return new RelayCommand<string>(Edit); } }
        private void Edit(string pageName)
        {
            navigationService.Navigate(pageName);
        }
        #endregion
    }
}
