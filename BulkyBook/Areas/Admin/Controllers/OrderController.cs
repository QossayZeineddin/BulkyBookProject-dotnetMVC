using BulkyBook.Areas.Customer.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Uitility;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult getAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.orderHeader.getAll(includeProperies: "applecationUser");
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