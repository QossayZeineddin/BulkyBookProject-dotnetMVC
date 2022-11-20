using System.Security.Claims;
using BulkyBook.Areas.Admin.Models.ViewModels;
using BulkyBook.Areas.Customer.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Uitility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Linq;

using RefundService = Stripe.TestHelpers.RefundService;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty] public OrderVM orderVm { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Deteils(int OrderId)
        {
            orderVm = new OrderVM()
            {
                orderHeader =
                    _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == OrderId,
                        includeProperies: "applecationUser"),
                orderDetails =
                    _unitOfWork.orderDetail.getAllByUserId(u => u.orderId == OrderId, includeProperies: "product")
            };
            if (orderVm.orderDetails == null || orderVm.orderHeader == null)
            {
                TempData["error"] = "The Order Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(orderVm);
        }

        [ActionName("Deteils")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            orderVm.orderHeader = _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == orderVm.orderHeader.id,
                includeProperies: "applecationUser");
            orderVm.orderDetails =
                _unitOfWork.orderDetail.getAllByUserId(u => u.orderId == orderVm.orderHeader.id, includeProperies: "product");

            //stripe settings 
            var domain = "https://localhost:7143/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={orderVm.orderHeader.id}",
                CancelUrl = domain + $"admin/order/details?orderId={orderVm.orderHeader.id}",
            };

            foreach (var item in orderVm.orderDetails)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.price * 100),//20.00 -> 2000
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Title
                        },

                    },
                    Quantity = item.count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.orderHeader.updateStripePayment(orderVm.orderHeader.id, session.Id, session.PaymentIntentId);
            _unitOfWork.save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult PaymentConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == orderHeaderid);
            if (orderHeader.paymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.orderHeader.updateStatus(orderHeaderid, orderHeader.orderStatus,
                        SD.PaymentStatusApproved);
                    _unitOfWork.save();
                }
            }

            return View(orderHeaderid);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult updateOrderDeteils()
        {
            var orderHeaderFromDb =
                _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == orderVm.orderHeader.id, tracked: false);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.name = orderVm.orderHeader.name;
                orderHeaderFromDb.city = orderVm.orderHeader.city;
                orderHeaderFromDb.PhoneNumber = orderVm.orderHeader.PhoneNumber;
                orderHeaderFromDb.streetAddress = orderVm.orderHeader.streetAddress;
                orderHeaderFromDb.State = orderVm.orderHeader.State;
                orderHeaderFromDb.postalCode = orderVm.orderHeader.postalCode;
                if (orderVm.orderHeader.carrier != null)
                {
                    orderHeaderFromDb.carrier = orderVm.orderHeader.carrier;
                }

                if (orderVm.orderHeader.trackingNumber != null)
                {
                    orderHeaderFromDb.trackingNumber = orderVm.orderHeader.trackingNumber;
                }

                _unitOfWork.orderHeader.update(orderHeaderFromDb);
                _unitOfWork.save();
                TempData["success"] = "Order Details update successfully!.";
                return RedirectToAction("Deteils", "Order", new { OrderId = orderHeaderFromDb.id }); 
            }
            else
            {
                TempData["error"] = "Order Not found !!!.";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult strartProcessing()
        {
            _unitOfWork.orderHeader.updateStatus(orderVm.orderHeader.id, SD.StatusInProcess);
            _unitOfWork.save();
            TempData["success"] = "Order Statuse update successfully!.";
            return RedirectToAction("Deteils", "Order", new { OrderId = orderVm.orderHeader.id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult shipOrder()
        {
            var orderHeaderFromDb =
                _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == orderVm.orderHeader.id, tracked: false);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.trackingNumber = orderVm.orderHeader.trackingNumber;
                orderHeaderFromDb.carrier = orderVm.orderHeader.carrier;
                orderHeaderFromDb.orderStatus = SD.StatusShipped;
                orderHeaderFromDb.shoppingDate = DateTime.Now;
                if (orderHeaderFromDb.paymentStatus == SD.PaymentStatusDelayedPayment)
                {
                    orderHeaderFromDb.paymentDueDate = DateTime.Now.AddDays(30);
                }

                _unitOfWork.orderHeader.update(orderHeaderFromDb);
                _unitOfWork.save();
                TempData["success"] = "Order shipped  successfully!.";
                return RedirectToAction("Deteils", "Order", new { OrderId = orderVm.orderHeader.id });
            }
            else
            {
                TempData["error"] = "Order Not found !!!.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderHeaderFromDb =
                _unitOfWork.orderHeader.getFirstOrDefault(u => u.id == orderVm.orderHeader.id, tracked: false);
            if (orderHeaderFromDb.paymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId
                };

                var service = new RefundService();
                //   Refund refund = service.Create(options);

                _unitOfWork.orderHeader.updateStatus(orderHeaderFromDb.id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.orderHeader.updateStatus(orderHeaderFromDb.id, SD.StatusCancelled, SD.StatusCancelled);
            }

            _unitOfWork.save();

            TempData["success"] = "Order Cancelled Successfully.";
            return RedirectToAction("Deteils", "Order", new { orderId = orderVm.orderHeader.id });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult getAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.orderHeader.getAll(includeProperies: "applecationUser");
            }
            else
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.orderHeader.getAllByUserId(u => u.applecationUserId == claim.Value,
                    includeProperies: "applecationUser");
            }


            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.paymentStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.orderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.orderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.orderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}