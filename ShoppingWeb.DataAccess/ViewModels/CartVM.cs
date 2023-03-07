using ShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.DataAccess.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Models.Cart> ListOfCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
