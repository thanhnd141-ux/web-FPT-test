using G1FoodLibrary.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(ProductDTO product) => ProductDAO.Instance.AddProduct(product);

        public void DeleteProduct(string productID) => ProductDAO.Instance.DeleteProduct(productID);

        public string GetNewProductID(string productName) => ProductDAO.Instance.GetNewProductID(productName);

        public ProductDTO GetProductByProductID(string productID) => ProductDAO.Instance.GetProductDTOByProductID(productID);

        public IEnumerable<ProductDTO> GetProductHome() => ProductDAO.Instance.GetProducts();

        public void UpdateProduct(ProductDTO product) => ProductDAO.Instance.UpdateProduct(product);
    }
}
