using ShoppingWeb.DataAccess.Data;
using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.DataAccess.Repositories
{
    public class ProductRepositoy : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepositoy(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var productDb = _context.Products.FirstOrDefault(x=> x.Id== product.Id);
            if (productDb != null)
            {
                productDb.Name= product.Name;
                productDb.Description= product.Description;
                productDb.Price= product.Price;
                if (productDb.ImageUrl != null)
                {
                    productDb.ImageUrl = product.ImageUrl;
                }
            }
        }

    }
}
