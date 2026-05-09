namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
