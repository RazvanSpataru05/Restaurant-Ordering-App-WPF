using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class CategoryWithProducts
    {
        public string Category { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public CategoryWithProducts(string category, ObservableCollection<Product> products) 
        {
            Category = category;
            Products = products;
        }
    }
}
