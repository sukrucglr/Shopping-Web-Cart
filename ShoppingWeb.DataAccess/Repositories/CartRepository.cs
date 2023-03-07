using ShoppingWeb.DataAccess.Data;
using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.DataAccess.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void DecrementCartItem(Cart cart, int count)
        {
            var cartDb = _context.Carts.FirstOrDefault(x => x.Id == cart.Id);
            if (cartDb != null)
            {
                cartDb.Count = cartDb.Count - count;
                _context.Carts.Update(cartDb);
                _context.SaveChanges();
            }
        }

        public void IncrementCartItem(Cart cart, int count)
        {
            var cartDb = _context.Carts.FirstOrDefault(x => x.Id == cart.Id);
            if (cartDb != null)
            {
                cartDb.Count = cartDb.Count + count;
                _context.Carts.Update(cartDb);
                _context.SaveChanges();
            }
        }

        public void Update(Cart cart)
        {
            var cartDb = _context.Carts.FirstOrDefault(x => x.Id == cart.Id);
            if (cartDb != null)
            {
                _context.Carts.Update(cart);

            }
            else
            {
                _context.Carts.Add(cart);
            }
            _context.SaveChanges();
        }
    }
}
