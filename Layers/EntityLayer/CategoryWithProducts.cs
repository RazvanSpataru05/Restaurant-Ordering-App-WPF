using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class CategoryWithProducts
    {
        public Category Category { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public CategoryWithProducts(Category category, ObservableCollection<Product> products) 
        {
            Category = category;
            Products = products;
        }
    }
}
