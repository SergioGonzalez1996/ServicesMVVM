using GalaSoft.MvvmLight.Command;
using ServicesMVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ServicesMVVM.ViewModels
{
    public class QueriesViewModel : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<QueriesViewModel> Queries { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Total { get; set; }

        public DateTime DateQuery { get; set; }

        public int ServiceId { get; set; }

        public DateTime DateService { get; set; }

        public DateTime DateRegistered { get; set; }

        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Value { get { return Price * (decimal)Quantity; } }
        #endregion

        #region Constructors
        public QueriesViewModel()
        {
            DateQuery = DateTime.Today;
            Queries = new ObservableCollection<QueriesViewModel>();
        }
        #endregion

        #region Commands
        public ICommand QueryCommand { get { return new RelayCommand(Query); } }
        private void Query()
        {
            using (var da = new DataAccess())
            {
                var services = da.GetList<Service>(true)
                                   .Where(s => s.DateRegistered.Year == DateQuery.Year &&
                                    s.DateRegistered.Month == DateQuery.Month &&
                                    s.DateRegistered.Day == DateQuery.Day)
                                    .OrderByDescending(s => s.DateService)
                                    .ToList();
                Queries.Clear();
                Total = 0;
                //Total = services.Sum(s => s.Value); // 1. This doesn't work, remember to ask to Zulu
                Total = services.Sum(s => (decimal)s.Quantity * s.Product.Price); // Test 1, please ignore this [Works]
                foreach (var service in services)
                {
                    //Total = Total + ((decimal)service.Quantity * service.Product.Price); // Test 2, please ignore this [Works]
                    Queries.Add(new QueriesViewModel
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
                OnPropertyChanged("Total");
            }
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
