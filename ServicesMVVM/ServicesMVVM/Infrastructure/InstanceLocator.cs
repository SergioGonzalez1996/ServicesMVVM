using ServicesMVVM.ViewModels;

namespace ServicesMVVM.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public ProductsViewModel Products { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
            Products = new ProductsViewModel();
        }
    }
}
