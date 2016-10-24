using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;

namespace ServicesMVVM.Models
{
    public class Service
    {
        [PrimaryKey, AutoIncrement]
        public int ServiceId { get; set; }

        public DateTime DateService { get; set; }

        public DateTime DateRegistered { get; set; }

        public int ProductId { get; set; }

        [ManyToOne]
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public decimal Value { get { return Price * (decimal)Quantity; } }

        public override int GetHashCode()
        {
            return ServiceId;
        }

        public override string ToString()
        {
            return string.Format("{0} {1:d} {2:C2}", ServiceId, DateService, Value);
        }

    }
}
