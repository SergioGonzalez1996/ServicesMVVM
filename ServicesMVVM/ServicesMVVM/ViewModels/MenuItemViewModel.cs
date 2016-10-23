using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Pages;
using ServicesMVVM.Services;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    class MenuItemViewModel
    {
        private NavigationService navigationService;

        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
        }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(() => navigationService.Navigate(PageName));
            }
        }
    }
}
