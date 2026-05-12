namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? MenuId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
        public string? MenuName { get; set; }
        public string? ProductImagePath { get; set; }
        public string? MenuImagePath { get; set; }
    }
}
