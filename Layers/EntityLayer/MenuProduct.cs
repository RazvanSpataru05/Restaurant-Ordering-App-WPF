namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class MenuProduct
    {
        public int MenuId { get; set; }
        public int ProductId { get; set; }
        public string PortionQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
