using System.Threading.Tasks;

namespace ServicesMVVM.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await App.Navigator.DisplayAlert(title, message, "OK");
        }

    }
}
