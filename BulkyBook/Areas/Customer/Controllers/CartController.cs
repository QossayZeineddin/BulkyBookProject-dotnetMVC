using BulkyBook.Areas.Customer.Models;
using BulkyBook.Areas.Customer.Models.ViewModels;
using BulkyBook.Repository.IRepository;
using BulkyBook.Uitility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var clim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingCartsList = _unitOfWork.shoppingCart.getAllByUserId(u => u.applecationUserId == clim.Value,
                    includeProperies: "Product"),
                orderHeader = new()
            };
            ShoppingCartVM.orderHeader.applecationUser =
                _unitOfWork.applcationUser.getFirstOrDefault(u => u.Id == clim.Value);
            ViewData["userName"] = ShoppingCartVM.orderHeader.applecationUser.name;

            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.priceTotal = getPriceBasedOnQuantity(cart.count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                ShoppingCartVM.orderHeader.orderTotal += (cart.priceTotal * cart.count);
            }

            return View(ShoppingCartVM);
        }

        

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.shoppingCart.IncrementCount(cartFromDb, 1);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.shoppingCart.DecrementCount(cartFromDb, 1);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.shoppingCart.remove(cartFromDb);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingCartsList = _unitOfWork.shoppingCart.getAllByUserId(u => u.applecationUserId == claim.Value,
                    includeProperies: "Product"),
                orderHeader = new()
            };
            ShoppingCartVM.orderHeader.applecationUser =
                _unitOfWork.applcationUser.getFirstOrDefault(u => u.Id == claim.Value);

            ShoppingCartVM.orderHeader.name = ShoppingCartVM.orderHeader.applecationUser.name;
            ShoppingCartVM.orderHeader.PhoneNumber = ShoppingCartVM.orderHeader.applecationUser.PhoneNumber;
            ShoppingCartVM.orderHeader.streetAddress = ShoppingCartVM.orderHeader.applecationUser.streetAddress;
            ShoppingCartVM.orderHeader.city = ShoppingCartVM.orderHeader.applecationUser.city;
            ShoppingCartVM.orderHeader.State = ShoppingCartVM.orderHeader.applecationUser.State;
            ShoppingCartVM.orderHeader.postalCode = ShoppingCartVM.orderHeader.applecationUser.postalCode;

            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.priceTotal = getPriceBasedOnQuantity(cart.count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                ShoppingCartVM.orderHeader.orderTotal += (cart.priceTotal * cart.count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ActionName("Summary")]
        public IActionResult SummaryPost(ShoppingCartVM shoppingCartVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM.shoppingCartsList = _unitOfWork.shoppingCart.getAllByUserId(
                u => u.applecationUserId == claim.Value,
                includeProperies: "Product");

            shoppingCartVM.orderHeader.paymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.orderHeader.orderStatus = SD.StatusPending;
            shoppingCartVM.orderHeader.orderDate = DateTime.Now;
            shoppingCartVM.orderHeader.applecationUserId = claim.Value;


            foreach (var cart in shoppingCartVM.shoppingCartsList)
            {
                cart.priceTotal = getPriceBasedOnQuantity(cart.count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                shoppingCartVM.orderHeader.orderTotal += (cart.priceTotal * cart.count);
            }

            shoppingCartVM.orderHeader.trackingNumber = "1";
            _unitOfWork.orderHeader.add(shoppingCartVM.orderHeader);
            _unitOfWork.save();

            foreach (var cart in shoppingCartVM.shoppingCartsList)
            {
                OrderDetail orderDetail = new()
                {
                    productId = cart.productId,
                    orderId = shoppingCartVM.orderHeader.id,
                    price = cart.priceTotal,
                    count = cart.count
                };
                _unitOfWork.orderDetail.add(orderDetail);
                _unitOfWork.save();
            }

            var domain = "https://localhost:7143/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>()
                ,
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartVM.orderHeader.id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in shoppingCartVM.shoppingCartsList)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.priceTotal * 100),//20.00 -> 2000
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },

                    },
                    Quantity = item.count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.orderHeader.updateStripePayment(shoppingCartVM.orderHeader.id, session.Id, session.PaymentIntentId);
            _unitOfWork.save();
            
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

           /* _unitOfWork.shoppingCart.removeRange(shoppingCartVM.shoppingCartsList);
            _unitOfWork.save();
            return RedirectToAction("Index", "Home");*/
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.updateStatus(orderHeader.id , SD.StatusApproved ,SD.PaymentStatusApproved);
                _unitOfWork.save();
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.shoppingCart.getAllByUserId(u => u.applecationUserId ==
                orderHeader.applecationUserId).ToList();

           

            _unitOfWork.shoppingCart.removeRange(shoppingCarts);
            _unitOfWork.save();
            return View(id);
        }
        
        private double getPriceBasedOnQuantity(double qountity, double price, double price50, double price100)
        {
            if (qountity <= 50)
            {
                return (price);
            }
            else if (50 < qountity && qountity <= 100)
            {
                return (price50);
            }
            else if (qountity > 100)
            {
                return (price100);
            }
            else
            {
                return 0;
            }
        }
        
    }
    
}