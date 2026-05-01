using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.DBObjects
{
    public class MenuProduct
    {
        public int MenuId { get; set; }
        public int ProductId { get; set; }
        public string PortionQuantity { get; set; }
    }
}
