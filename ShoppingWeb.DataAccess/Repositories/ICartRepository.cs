using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.DataAccess.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        void Update(Cart cart);
        void IncrementCartItem(Cart cart,int count);
        void DecrementCartItem(Cart cart, int v);
    }
}