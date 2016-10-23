using ServicesMVVM.ViewModels;

namespace ServicesMVVM.Infrastructure
{
    class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
