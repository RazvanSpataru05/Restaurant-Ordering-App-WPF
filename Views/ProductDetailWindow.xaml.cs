using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class ProductDetailWindow : Window
    {
        public ProductDetailWindow(ProductDetailVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
