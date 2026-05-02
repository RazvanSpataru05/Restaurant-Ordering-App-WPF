namespace RestaurantOrderingApp.Layers.EntityLayer
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
