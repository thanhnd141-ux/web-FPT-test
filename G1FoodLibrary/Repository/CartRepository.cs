using G1FoodLibrary.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public class CartRepository : ICartRepository
    {
        public void AddCart(CartDTO cartDTO) => CartDAO.Instance.AddCart(cartDTO);

        public void DeleteCart(int cartID) => CartDAO.Instance.DeleteCart(cartID);

        public IEnumerable<CartDTO> GetListCartByAccountID(string accountID) => CartDAO.Instance.GetCartDTOByAccountID(accountID);
    }
}
