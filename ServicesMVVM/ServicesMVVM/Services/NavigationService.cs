using System;
using ServicesMVVM.Pages;

namespace ServicesMVVM.Services
{
    public class NavigationService
    {
        public async void Navigate(string PageName)
        {
            App.Master.IsPresented = false;

            switch (PageName)
            {
                case "ProductsPage":
                    await App.Navigator.PushAsync(new ProductsPage());
                    break;
                case "QueriesPage":
                    await App.Navigator.PushAsync(new QueriesPage());
                    break;
                case "NewServicePage":
                    await App.Navigator.PushAsync(new NewServicePage());
                    break;
                case "NewProductPage":
                    await App.Navigator.PushAsync(new NewProductPage());
                    break;
                case "MainPage":
                    await App.Navigator.PopToRootAsync();
                    break;
                default:
                    break;
            }
        }

        internal void SetMainPage()
        {
            App.Current.MainPage = new MasterPage();
        }
    }
}
