using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Repository
{
    public interface ICartRepository
    {
        public IEnumerable<CartDTO> GetListCartByAccountID(string accountID);
        public void AddCart(CartDTO cartDTO);
        public void DeleteCart(int cartID);
    }
}
