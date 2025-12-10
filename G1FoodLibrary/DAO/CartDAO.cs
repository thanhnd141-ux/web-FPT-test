using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1FoodLibrary.Context;
using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace G1FoodLibrary.DAO
{
    public class CartDAO
    {
        private DBContext _context;
        private static CartDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CartDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartDAO();
                    }
                    return instance;
                }
            }
        }

        public CartDAO() => _context = new DBContext();

        public IEnumerable<CartDTO> GetCartDTOByAccountID(string accountID)
        {
            var carts = new List<Cart>();
            try
            {
                carts = _context.Carts.Include(c => c.Product).Where(c => c.AccountId == accountID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var cartDTOs = new List<CartDTO>();

            foreach (var cart in carts)
            {
                cartDTOs.Add(new CartDTO
                {
                    CartId = cart.CartId,
                    CartQuantity = cart.CartQuantity,
                    ProductId = cart.ProductId,
                    ProductName = cart.Product.ProductName,
                    ProductPrice = cart.Product.ProductPrice,
                    ProductSalePercent = cart.Product.ProductSalePercent,
                    ProductImage = cart.Product.ProductImage
                });
            }

            return cartDTOs;
        }

        public Cart GetCartByCartID(int cartID)
        {
            Cart cart = null;
            try
            {
                cart = _context.Carts.Where(c => c.CartId == cartID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cart;
        }

        public Cart GetCartByProductID(string productID)
        {
            Cart cart = null;
            try
            {
                cart = _context.Carts.Where(c => c.ProductId.Equals(productID)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cart;
        }

        public void AddCart(CartDTO cartDTO)
        {
            try
            {
                Cart _cart = GetCartByProductID(cartDTO.ProductId);
                if (_cart == null)
                {
                    Cart cart = new Cart
                    {
                        CartId = cartDTO.CartId,
                        CartQuantity = cartDTO.CartQuantity,
                        ProductId = cartDTO.ProductId,
                        AccountId = cartDTO.AccountId
                    };

                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                } else
                {
                    _cart.CartQuantity = _cart.CartQuantity + cartDTO.CartQuantity;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCart(int cartID)
        {
            try
            {
                Cart _cart = GetCartByCartID(cartID);
                if (_cart != null)
                {
                    _context.Carts.Remove(_cart);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The cart does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void DeleteAllCart(string accountID)
        {
            try
            {
                List<Cart> carts = _context.Carts.Where(c => c.AccountId.Equals(accountID)).ToList();

                if (carts != null)
                {
                    _context.Carts.RemoveRange(carts);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The cart does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
