using DataAccess.DTO;
using G1FoodLibrary.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void CreateOrder(OrderDTO orderDTO) => OrderDAO.Instance.AddOrder(orderDTO);

        public void CreateOrderDetails(OrderDTO orderDTO) => OrderDAO.Instance.AddOrderDetails(orderDTO);

        public IEnumerable<OrderDTO> GetAllOrder() => OrderDAO.Instance.GetAllOrder();

        public IEnumerable<OrderDTO> GetListOrderByAccChef(string accountID) => OrderDAO.Instance.GetAllOrderByAccChef(accountID);

        public IEnumerable<OrderDTO> GetListOrderByAccountID(string accountID) => OrderDAO.Instance.GetAllOrderByAccountID(accountID);

        public IEnumerable<OrderDTO> GetListOrderByAccShip(string accountID) => OrderDAO.Instance.GetAllOrderByAccShip(accountID);

        public IEnumerable<OrderDetailsDTO> GetListOrderDetailsByOrderID(string orderID) => OrderDAO.Instance.GetAllOrderDetailsByOrderID(orderID);

        public IEnumerable<OrderDTO> GetListOrderProcessing() => OrderDAO.Instance.GetListOrderByStatus("PROCESSING");

        public OrderDTO GetOrderByOrderID(string orderID) => OrderDAO.Instance.GetAllOrderByOrderID(orderID);

        public string IsDelivering(string accountID) => OrderDAO.Instance.isDelivering(accountID);

        public void UpdateOrderStatus(string orderID, string accountID, string orderStatus) => OrderDAO.Instance.UpdateOrderStatus(orderID, accountID, orderStatus);
    }
}
