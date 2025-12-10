using DataAccess.DTO;
using DataAccess.Entities;
using G1FoodLibrary.Context;
using G1FoodLibrary.Hash;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.DAO
{
    public class OrderDAO
    {
        private DBContext _context;
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public OrderDAO() => _context = new DBContext();

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }
        public string GetLastOrderID()
        {
            var connectionString = GetConnectionString();
            var sqlQuery = @"SELECT TOP 1 [OrderID]
                     FROM [Order]
                     ORDER BY CAST(RIGHT(OrderID, 4) AS INT) DESC";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        public string GetNewOrderID()
        {
            GenerageID g = new GenerageID();
            string orderID = g.GenerateNewID("OR", GetLastOrderID());

            return orderID;
        }

        public string GetLastOrderDetailsID()
        {
            var connectionString = GetConnectionString();
            var sqlQuery = @"SELECT TOP 1 OrderDID
                     FROM ORDER_DETAIL
                     ORDER BY CAST(RIGHT(OrderDID, 4) AS INT) DESC";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        public string GetNewOrderDetailsID()
        {
            GenerageID g = new GenerageID();
            string orderID = g.GenerateNewID("OD", GetLastOrderDetailsID());

            return orderID;
        }

        public void AddOrder(OrderDTO orderDTO)
        {
            DateTime now = DateTime.Now;
            try
            {
                string note = " ";
                if (orderDTO.OrderNote != null)
                {
                    note = orderDTO.OrderNote;
                }
                

                Order order = new Order
                {
                    OrderId = GetNewOrderID(),
                    OrderNote = note,
                    OrderStatus = "PROCESSING",
                    OrderDate = now,
                    AccountId = orderDTO.AccountId,
                    BuyerFullName = orderDTO.BuyerFullName,
                    BuyerPhone = orderDTO.BuyerPhone,
                    BuyerAddress = orderDTO.BuyerAddress,
                    VoucherId = "",
                    ProductSalePercent = 0,
                    AccIdofChef = "",
                    AccIdofShipper = "SP0008"
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddOrderDetails(OrderDTO orderDTO)
        {
            List<CartDTO> cartDTOs = new CartDAO().GetCartDTOByAccountID(orderDTO.AccountId).ToList();

            try
            {
                foreach (CartDTO cartDTO in cartDTOs)
                {
                    OrderDetail orderDetails = new OrderDetail
                    {
                        OrderDid = GetNewOrderDetailsID(),
                        OrderDquantity = cartDTO.CartQuantity,
                        OrderDprice = cartDTO.ProductPrice,
                        ProductId = cartDTO.ProductId,
                        OrderId = GetLastOrderID()
                    };

                    _context.OrderDetails.Add(orderDetails);
                    _context.SaveChanges();
                }

                new CartDAO().DeleteAllCart(orderDTO.AccountId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<OrderDTO> GetAllOrder()
        {
            var orders = _context.Orders.ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            
            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    OrderId = order.OrderId,
                    BuyerFullName = order.BuyerFullName,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus
                });
            }

            return orderDTOs;
        }

        public void UpdateOrderStatus(string orderID, string accountID, string orderStatus)
        {
            var order = _context.Orders.Where(o => o.OrderId.Equals(orderID)).FirstOrDefault();

            order.OrderStatus = orderStatus;
            if (accountID.Contains("CH")) {
                order.AccIdofChef = accountID;
            } else
            {
                order.AccIdofShipper = accountID;
            }
            _context.SaveChanges();
        }

        public IEnumerable<OrderDTO> GetListOrderByStatus(string status)
        {
            var orders = _context.Orders.Where(o => o.OrderStatus.Equals(status)).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    OrderId = order.OrderId,
                    BuyerFullName = order.BuyerFullName,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus
                });
            }

            return orderDTOs;
        }

        public IEnumerable<OrderDetailsDTO> GetAllOrderDetailsByOrderID(string orderID)
        {
            var orders = _context.OrderDetails.Include(o => o.Product).Where(o => o.OrderId.Equals(orderID)).ToList();

            List<OrderDetailsDTO> orderDTOs = new List<OrderDetailsDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDetailsDTO
                {
                    OrderId = order.OrderId,
                    OrderDid = order.OrderDid,
                    OrderDprice = order.OrderDprice,
                    OrderDquantity = order.OrderDquantity,
                    ProductId = order.ProductId,
                    ProductName = order.Product.ProductName,
                    ProductImage = order.Product.ProductImage
                });
            }

            return orderDTOs;
        }

        public IEnumerable<OrderDTO> GetAllOrderByAccountID(string accountID)
        {
            var orders = _context.Orders.Where(o => o.AccountId.Equals(accountID)).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    OrderNote = order.OrderNote,
                    AccountId = order.AccountId,
                    OrderStatus = order.OrderStatus,
                    BuyerFullName = order.BuyerFullName,
                    BuyerAddress = order.BuyerAddress,
                    BuyerPhone = order.BuyerPhone
                });
            }

            return orderDTOs;
        }

        public IEnumerable<OrderDTO> GetAllOrderByAccShip(string accountID)
        {
            var orders = _context.Orders.Where(o => o.AccIdofShipper.Equals(accountID)).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    OrderNote = order.OrderNote,
                    AccountId = order.AccountId,
                    OrderStatus = order.OrderStatus,
                    BuyerFullName = order.BuyerFullName,
                    BuyerAddress = order.BuyerAddress,
                    BuyerPhone = order.BuyerPhone
                });
            }

            return orderDTOs;
        }

        public IEnumerable<OrderDTO> GetAllOrderByAccChef(string accountID)
        {
            var orders = _context.Orders.Where(o => o.AccIdofChef.Equals(accountID)).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    OrderNote = order.OrderNote,
                    AccountId = order.AccountId,
                    OrderStatus = order.OrderStatus,
                    BuyerFullName = order.BuyerFullName,
                    BuyerAddress = order.BuyerAddress,
                    BuyerPhone = order.BuyerPhone
                });
            }

            return orderDTOs;
        }


        public OrderDTO GetAllOrderByOrderID(string orderID)
        {
            var orders = _context.Orders.Where(o => o.OrderId.Equals(orderID)).FirstOrDefault();

            OrderDTO orderDTOs = new OrderDTO
            {
                OrderId = orderID,
                OrderDate = orders.OrderDate,
                OrderNote = orders.OrderNote,
                AccountId = orders.AccountId,
                OrderStatus = orders.OrderStatus,
                BuyerFullName = orders.BuyerFullName,
                BuyerAddress = orders.BuyerAddress,
                BuyerPhone = orders.BuyerPhone,
                AccIdofChef = orders.AccIdofChef,
                AccIdofShipper = orders.AccIdofShipper
            };

            return orderDTOs;
        }

        public string isDelivering(string accountID)
        {
            var order = _context.Orders.Where(o => o.AccIdofShipper.Equals(accountID) && o.OrderStatus.Equals("DELIVERING")).FirstOrDefault();

            if (order != null)
            {
                return order.OrderId;
            }

            return "Not Delivering";
        }
    }
}
