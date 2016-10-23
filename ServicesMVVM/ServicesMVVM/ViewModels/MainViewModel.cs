using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    class MainViewModel
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<ServicesViewModel> Services { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            navigationService = new NavigationService();
            LoadMenu();
            LoadServices();
        }
        #endregion

        #region Commands
        public ICommand GoToCommand { get { return new RelayCommand<string>(GoTo); } }
        private void GoTo(string pageName)
        {
            navigationService.Navigate(pageName);
        }

        public ICommand StartCommand { get { return new RelayCommand(Start); } }
        private void Start()
        {
            navigationService.SetMainPage();
        }
        #endregion

        #region Methods
        private void LoadServices()
        {
            Services = new ObservableCollection<ServicesViewModel>();
            for (int i = 0; i < 10; i++)
            {
                Services.Add(new ServicesViewModel
                {
                    ServiceId = i,
                    ProductId = "Producto: " + (i * 2),
                    Quantity = i * 3,
                    Price = i * 4,
                    DateService = DateTime.Today,
                    DateRegistered = DateTime.Today,
                });
            }
        }

        private void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

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
