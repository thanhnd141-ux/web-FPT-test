using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1FoodLibrary.Hash;
using Microsoft.Data.SqlClient;
using G1FoodLibrary.Context;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace G1FoodLibrary.DAO
{
    public class ProductDAO
    {
        private DBContext _context;
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public ProductDAO() => _context = new DBContext();

        public IEnumerable<ProductDTO> GetProducts()
        {
            var products = new List<Product>();
            string productStatus = "REMOVED";

            try
            {
                products = _context.Products.Where(p => !p.ProductStatus.Equals(productStatus)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }       

            var productDTOs = products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductStatus = p.ProductStatus,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductSalePercent = p.ProductSalePercent,
                CategoryId = p.CategoryId
            });

            return productDTOs;
        }

        public ProductDTO GetProductDTOByProductID(string productId)
        {
            string productStatus = "REMOVED";
            Product product = null;
            try
            {
                product = _context.Products.FirstOrDefault(p => !p.ProductStatus.Equals(productStatus) && p.ProductId == productId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            ProductDTO productDTO = new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductStatus = product.ProductStatus,
                ProductImage = product.ProductImage,
                ProductPrice = product.ProductPrice,
                ProductSalePercent = product.ProductSalePercent,
                CategoryId = product.CategoryId
            };

            return productDTO;
        }

        public Product GetProductByProductID(string productId)
        {
            Product product = null;
            try
            {
                product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return product;
        }

        public void AddProduct(ProductDTO productDTO)
        {
            try
            {
                Product _product = GetProductByProductID(productDTO.ProductId);
                if (_product == null)
                {
                    Product product = new Product
                    {
                        ProductName = productDTO.ProductName,
                        ProductDescription = productDTO.ProductDescription,
                        ProductStatus = productDTO.ProductStatus,
                        ProductImage = productDTO.ProductImage,
                        ProductPrice = productDTO.ProductPrice,
                        ProductId = productDTO.ProductId,
                        ProductSalePercent = productDTO.ProductSalePercent,
                        CategoryId = productDTO.CategoryId
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The productID is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(ProductDTO productDTO)
        {
            try
            {
                Product _product = GetProductByProductID(productDTO.ProductId);
                if (_product != null)
                {
                    _context.Entry(_product).CurrentValues.SetValues(productDTO);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The productID is not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteProduct(string productID)
        {
            try
            {
                Product _product = GetProductByProductID(productID);
                if (_product != null)
                {
                    _product.ProductStatus = "REMOVED";
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }
        public string GetLastProductID()
        {
            var connectionString = GetConnectionString();
            var sqlQuery = @"SELECT TOP 1 ProductID
                     FROM Product
                     ORDER BY CAST(RIGHT(ProductId, 4) AS INT) DESC";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        public string GetNewProductID(string productName)
        {
            GenerageID g = new GenerageID();
            string productID = g.GenerateNewID(g.GetPrefixFromProductName(productName), GetLastProductID());

            return productID;
        }
    }
}
