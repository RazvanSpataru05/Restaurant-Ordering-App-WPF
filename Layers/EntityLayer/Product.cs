using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PortionQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public string CategoryName { get; set; }
        public string? ImagePath { get; set; }
        public ObservableCollection<Allergen> Allergens { get; set; }
    }
}
