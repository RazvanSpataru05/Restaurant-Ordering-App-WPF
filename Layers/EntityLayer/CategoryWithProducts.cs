using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class CategoryWithProducts
    {
        public Category Category { get; set; }
        public ObservableCollection<ProductDisplay> Products { get; set; }

        public CategoryWithProducts(Category category, ObservableCollection<ProductDisplay> products) 
        {
            Category = category;
            Products = products;
        }
    }
}
