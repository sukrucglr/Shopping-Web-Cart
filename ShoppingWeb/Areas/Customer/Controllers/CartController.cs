using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWeb.DataAccess.Repositories;
using ShoppingWeb.DataAccess.ViewModels;
using ShoppingWeb.Models;
using System.Security.Claims;

namespace ShoppingWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CartVM vm { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            vm = new CartVM()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            foreach (var item in vm.ListOfCart)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(vm);
        }
        public IActionResult ComplateOrder(CartVM cartvm)
        {
            _unitOfWork.OrderHeader.Add(cartvm.OrderHeader);
            foreach (var item in cartvm.ListOfCart)
            {
                _unitOfWork.OrderDetail.Add(new OrderDetail { ProductId = item.ProductId, OrderHeaderId = cartvm.OrderHeader.Id, Price = item.Product.Price, Count = item.Count });
                _unitOfWork.Cart.Delete(item);
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            vm = new CartVM()
            {
                ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            vm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetT(x => x.Id == claims.Value);
            vm.OrderHeader.Name = vm.OrderHeader.ApplicationUser.Name;
            vm.OrderHeader.Phone = vm.OrderHeader.ApplicationUser.PhoneNumber;
            vm.OrderHeader.Address = vm.OrderHeader.ApplicationUser.Adress;
            vm.OrderHeader.City = vm.OrderHeader.ApplicationUser.City;
            vm.OrderHeader.State = vm.OrderHeader.ApplicationUser.State;
            vm.OrderHeader.PostalCode = vm.OrderHeader.ApplicationUser.PinCode;
            foreach (var item in vm.ListOfCart)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(vm);
        }
        //[HttpPost]
        //public IActionResult Summary(CartVM vm)
        //{
        //    var cliamsIdentity = (ClaimsIdentity)User.Identity;
        //    var claims = cliamsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    vm.ListOfCart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product");
        //    vm.OrderHeader.OrderStatus = OrderStatus.StatusPending;
        //    vm.OrderHeader.PaymentStatus = PaymentStatus.StatusPending;
        //    vm.OrderHeader.DateOfOrder = DateTime.Now;
        //    vm.OrderHeader.ApplicationUser.Id = claims.Value;

        //    foreach (var item in vm.ListOfCart)
        //    {
        //        vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
        //    }
        //    _unitOfWork.OrderHeader.Add(vm.OrderHeader);
        //    _unitOfWork.Save();

        //    foreach (var item in vm.ListOfCart)
        //    {
        //        OrderDetail orderDetail = new OrderDetail()
        //        {
        //            ProductId = item.ProductId,
        //            OrderHeaderId = item.OrderHeaderId,
        //            Count = item.Product.Count,
        //            Count = item.Count,
        //            Price = item.Product.Price,
        //        };
        //        var domain = "Https://localhost:7129/";
        //        var options = new SessionCreateOptions
        //        {
        //            LineItems = new List<SessionLineItemOptions>(),
        //            Mode = "payment",
        //            SussessUrl = domain + $"Customer/Cart/ordersuccess?id={vm.OrderHeader.Id}",
        //            CancelUrl = domain + $"customer/cart/Index",
        //        };
        //        foreach (var cart in vm.ListOfCart)
        //        {
        //            var lineItemsOptions = new SessionLineItemOptions
        //            {
        //                PriceData = new SessionLineItemPriceDataOptions
        //                {
        //                    UnitAmount = (long)(item.Product.Price * 100),
        //                    Curency = "INR",
        //                    ProductData = new SessionLineItemPriceProductDataOptions
        //                    {
        //                        Name = item.Product.Name,
        //                    },
        //                },
        //                Quantity = item.Count,
        //            };
        //            options.LineItems.Add(lineItemsOptions);
        //        }
        //        var service = new SessionService();
        //        Session session = service.Create(options);
        //        _unitOfWork.OrderHeader.PaymentStatus(vm.OrderHeader.Id, session.Id, session.PaymentId);
        //        _unitOfWork.Save();

        //        _unitOfWork.Cart.DeleteRange(vm.ListOfCart);
        //        _unitOfWork.Save();

        //        Response.Headers.Add("Location", session.Url);
        //        return new StatusCodeResult(303);

        //        return RedirectToAction("Index", "Home");
        //    }


        //}
        //public IActionResult ordersuccess(int id)
        //{
        //    var orderHeader = _unitOfWork.OrderHeader.GetT(x => x.Id == id);
        //    var service = new SessionService();
        //    Session session = service.Get(orderHeader.SessionId);
        //    if (session.PaymentStatus.ToLower() == "paid")
        //    {
        //        _unitOfWork.OrderHeader.UpdateStatus(id, OrderStatus.StatusApproved, PaymentStatus.StatusApproved);
        //    }
        //    List<Models.Cart> cart = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == orderHeader.ApplicationId).ToList();
        //    _unitOfWork.Cart.DeleteRange(cart);
        //    _unitOfWork.Save();
        //    return View(id);
        //}

        public IActionResult plus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            _unitOfWork.Cart.IncrementCartItem(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            if (cart.Count <= 1)
            {
                _unitOfWork.Cart.Delete(cart);
            }
            else
            {
                _unitOfWork.Cart.DecrementCartItem(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cart = _unitOfWork.Cart.GetT(x => x.Id == id);
            _unitOfWork.Cart.Delete(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
