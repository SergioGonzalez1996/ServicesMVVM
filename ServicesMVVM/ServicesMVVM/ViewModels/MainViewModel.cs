using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using ServicesMVVM.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    public class MainViewModel
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<ServicesViewModel> Services { get; set; }
        public ObservableCollection<ProductsViewModel> Products { get; set; }
        public ProductsViewModel NewProduct { get; private set; }
        public ServicesViewModel NewService { get; private set; }
        public QueriesViewModel NewQuery { get; private set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();
            Services = new ObservableCollection<ServicesViewModel>();
            Products = new ObservableCollection<ProductsViewModel>();
            navigationService = new NavigationService();
            LoadMenu();
            LoadProducts();
        }
        #endregion

        #region Commands
        public ICommand GoToCommand { get { return new RelayCommand<string>(GoTo); } }
        private void GoTo(string pageName)
        {
            switch (pageName)
            {
                case "NewProductPage":
                    NewProduct = new ProductsViewModel();
                    break;
                case "NewServicePage":
                    NewService = new ServicesViewModel();
                    break;
                default:
                    break;
            }
            navigationService.Navigate(pageName);
        }

        public ICommand StartCommand { get { return new RelayCommand(Start); } }
        private void Start()
        {
            using (var da = new DataAccess())
            {
                var services = da.GetList<Service>(true)
                                   .Where(s => s.DateRegistered.Year == DateTime.Today.Year &&
                                    s.DateRegistered.Month == DateTime.Today.Month &&
                                    s.DateRegistered.Day == DateTime.Today.Day)
                                    .OrderByDescending(s => s.DateService)
                                    .ToList();
                //Services = new ObservableCollection<ServicesViewModel>();
                Services.Clear();
                foreach (var service in services)
                {
                    Services.Add(new ServicesViewModel
                    {
                        ServiceId = service.ServiceId,
                        ProductId = service.Product.ProductId,
                        ProductDescription = service.Product.Description,
                        Quantity = service.Quantity,
                        Price = service.Product.Price,
                        DateService = service.DateService,
                        DateRegistered = service.DateRegistered,
                    });
                }
            }
            NewQuery = new QueriesViewModel();
            navigationService.SetMainPage();
        }
        #endregion

        #region Methods
        private void LoadProducts()
        {
            using (var da = new DataAccess())
            {
                //Products = new ObservableCollection<ProductsViewModel>();
                Products.Clear();
                var products = da.GetList<Product>(false).OrderBy(p => p.Description);
                foreach (var product in products)
                {
                    Products.Add(new ProductsViewModel
                    {
                        ProductId = product.ProductId,
                        Description = product.Description,
                        Price = product.Price,
                    });
                }
            }
        }


        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_products.png",
                PageName = "ProductsPage",
                Title = "Productos",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_queries.png",
                PageName = "QueriesPage",
                Title = "Consultas",
            });
        }
        #endregion
    }
}
