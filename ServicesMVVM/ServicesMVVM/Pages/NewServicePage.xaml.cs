using Xamarin.Forms;
using ServicesMVVM.ViewModels;

namespace ServicesMVVM.Pages
{
    public partial class NewServicePage : ContentPage
    {
        private ServicesViewModel services;

        public NewServicePage()
        {
            InitializeComponent();
            services = new ServicesViewModel();
        }

        private void OnQuantityChanged (object sender, ValueChangedEventArgs e)
        {
            services.UpdateQuantity(sender, (int)e.NewValue);
        }
    }
}
