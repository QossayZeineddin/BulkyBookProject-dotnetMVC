using BulkyBook.Areas.Customer.Models;
using BulkyBook.Areas.Customer.Models.ViewModels;
using BulkyBook.Repository.IRepository;
using BulkyBook.Uitility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;
using System.Security.Claims;
using BulkyBook.PublicModels;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty] public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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
            TempData["success"] = "Add one successfully!";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.shoppingCart.DecrementCount(cartFromDb, 1);
            if (cartFromDb.count == 0)
            {
                _unitOfWork.shoppingCart.remove(cartFromDb);
            }

            _unitOfWork.save();
            TempData["success"] = "Remove one successfully!";

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
            if (ShoppingCartVM.shoppingCartsList.Count() == 0)
            {
                return RedirectToAction("Index");
            }

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


            shoppingCartVM.orderHeader.orderDate = DateTime.Now;
            shoppingCartVM.orderHeader.applecationUserId = claim.Value;


            foreach (var cart in shoppingCartVM.shoppingCartsList)
            {
                cart.priceTotal = getPriceBasedOnQuantity(cart.count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                shoppingCartVM.orderHeader.orderTotal += (cart.priceTotal * cart.count);
            }

            ApplecationUser applecationUser = _unitOfWork.applcationUser.getFirstOrDefault(u => u.Id == claim.Value);
            if (applecationUser.companyId != 1)
            {
                shoppingCartVM.orderHeader.paymentStatus = SD.PaymentStatusPending;
                shoppingCartVM.orderHeader.orderStatus = SD.StatusPending;
            }
            else
            {
                shoppingCartVM.orderHeader.paymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVM.orderHeader.orderStatus = SD.StatusApproved;
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

            if (applecationUser.companyId != 1)
            {
                var domain = "https://localhost:7143/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
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
                            UnitAmount = (long)(item.priceTotal * 100), //20.00 -> 2000
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

                _unitOfWork.orderHeader.updateStripePayment(shoppingCartVM.orderHeader.id, session.Id,
                    session.PaymentIntentId);
                _unitOfWork.save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.orderHeader.id });
            }

            /* _unitOfWork.shoppingCart.removeRange(shoppingCartVM.shoppingCartsList);
             _unitOfWork.save();
             return RedirectToAction("Index", "Home");*/
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader =
                _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == id, includeProperies: "applecationUser");
            if (orderHeader.paymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.orderHeader.updateStatus(orderHeader.id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.save();
                }
            }

            _emailSender.SendEmailAsync(orderHeader.applecationUser.Email, "New Order - Bulky Book",
                "<p>New Order Created</p>");

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


        #region API CALLS

        [HttpDelete]
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(u => u.Id == cartId);
            if (cartFromDb != null)
            {
                _unitOfWork.shoppingCart.remove(cartFromDb);
                _unitOfWork.save();
                TempData["success"] = "Product Deleted successfully!";
            }
            else
            {
                return NotFound();
            }

            return Json(new { success = true, message = "Delete Successfull" });
            //return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}