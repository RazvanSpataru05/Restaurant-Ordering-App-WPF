using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class OrderBLL
    {
        private readonly OrderDAL _orderDAL = new();

        public (int, string) CreateOrder(int userId, DateTime estimatedDeliveryTime, decimal totalPrice, decimal deliveryCost)
        {
            return _orderDAL.CreateOrder(userId, estimatedDeliveryTime, totalPrice, deliveryCost);
        }

        public ObservableCollection<Order> GetOrdersByUser(int userId)
        {
            return _orderDAL.GetOrdersByUser(userId);
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            _orderDAL.UpdateOrderStatus(orderId, status);
        }

        public void AddOrderItem(int orderId, int? productId, int? menuId, int quantity, decimal unitPrice)
        {
            _orderDAL.AddOrderItem(orderId, productId, menuId, quantity, unitPrice);
        }
        public ObservableCollection<OrderItem> GetOrderItems(int orderId)
        {
            return _orderDAL.GetOrderItems(orderId);
        }
    }
}
