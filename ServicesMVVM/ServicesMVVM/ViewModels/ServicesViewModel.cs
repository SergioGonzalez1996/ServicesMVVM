using System;

namespace ServicesMVVM.ViewModels
{
    class ServicesViewModel
    {
        public int ServiceId { get; set; }

        public DateTime DateService { get; set; }

        public DateTime DateRegistered { get; set; }

        public string ProductId { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public decimal Value { get { return Price * (decimal)Quantity; } }
    }
}
