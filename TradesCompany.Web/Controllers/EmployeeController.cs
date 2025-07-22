using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<Quotation> _quotationGRepository;
        private readonly IRepository<ServiceMan> _servicemanGRepository;
        private readonly IRepository<ServiceSchedule> _serviceGSchedule;
        private readonly IRepository<Booking> _bookingGRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeServices _employeeServices;
        private readonly IServiceManRepository _serviceManRepository;
        public EmployeeController(IBookingRepository bookingRepository,
                                  IRepository<Quotation> quotationGRepository,
                                  IRepository<ServiceMan> servicemanGRepository,
                                  IEmployeeServices employeeServices,
                                  IRepository<ServiceSchedule> serviceGSchedule,
                                  IServiceManRepository serviceManRepository,
                                  IRepository<Booking> bookingGRepository
                                 )
        {
            _bookingRepository = bookingRepository;
            _quotationGRepository = quotationGRepository;
            _servicemanGRepository = servicemanGRepository;
            _employeeServices = employeeServices;
            _serviceGSchedule = serviceGSchedule;
            _serviceManRepository = serviceManRepository;
            _bookingGRepository = bookingGRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var servicemen = await _serviceManRepository.GetServiceManByUserId(userId);
            var booking = await _bookingRepository.GetAllBookingAsync(servicemen.ServiceTypeId);
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> CreateQuotation(int bookingId)
        {
            // Show Quotation Form
            QuotationViewModel model = new QuotationViewModel
            {
                BookingId = bookingId,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuotation(QuotationViewModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var servicemen = await _serviceManRepository.GetServiceManByUserId(userId);
            var booking = await _bookingGRepository.GetByIdAsync(model.BookingId);

            if (ModelState.IsValid)
            {
                try
                {
                    // save DB
                    Quotation quotation = new Quotation
                    {
                        BookingId = model.BookingId,
                        ServiceManId = servicemen.Id,
                        Price = model.Price,
                        Status = "Pending",
                        QuotationPdf = "soon"
                    };
                    await _quotationGRepository.InsertAsync(quotation);
                    await _quotationGRepository.SaveAsync();
                    booking.Status = "Accepted";
                    await _bookingGRepository.SaveAsync();
                    // Create A PDF 
                    // Send Notification 
                }catch (Exception ex)
                {
                    // Throw Error 
                }
            }
            return RedirectToAction("CreateQuotation");
        }

        [HttpGet]
        public async Task<IActionResult> AllQuotation()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await _employeeServices.QuotationByServicerMan(userId);
            return View(data);
        }

        public async Task<IActionResult> ScheduleServices(int quotationId)
        {
            var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
            ServiceSchedule ss = new ServiceSchedule
            {
                BookingId = quotation.BookingId,
                ServiceManId = quotation.ServiceManId,
                QuotationId = quotation.Id,
                ScheduledAt = DateTime.Now,
                TotalPrice = quotation.Price,
                Status = "Scheduled"
            };

            quotation.Status = "Scheduled";
            await _quotationGRepository.SaveAsync();
            // save into serviceschedule 
            await _serviceGSchedule.InsertAsync(ss);
            await _serviceGSchedule.SaveAsync();
            // send notification 
            return Ok();
        }
        public async Task<IActionResult> MyScheduleServices()
        {
            // Fatch Data From DB 
            return View();
        }

        public IActionResult Notification()
        {
            return View();
        }
    }
}
