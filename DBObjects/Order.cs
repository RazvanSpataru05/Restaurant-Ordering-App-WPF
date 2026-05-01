using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.DBObjects
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DeliveryCost { get; set; }
        public string Status { get; set; }
    }
}
