using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    //[Authorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        private readonly IRepository<Quotation> _quotationGRepository;
        private readonly IRepository<ServiceMan> _servicemanGRepository;
        private readonly IRepository<ServiceSchedule> _serviceGSchedule;
        private readonly IRepository<Booking> _bookingGRepository;
        private readonly IRepository<Notification> _notificationGRepository;

        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeServices _employeeServices;
        private readonly IServiceManRepository _serviceManRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;
        private readonly IScheduleRepository _scheduleRepository;
        public EmployeeController(IBookingRepository bookingRepository,
                                  IRepository<Quotation> quotationGRepository,
                                  IRepository<ServiceMan> servicemanGRepository,
                                  IEmployeeServices employeeServices,
                                  IRepository<ServiceSchedule> serviceGSchedule,
                                  IServiceManRepository serviceManRepository,
                                  IRepository<Booking> bookingGRepository,
                                  IRepository<Notification> notificationGRepository,
                                  INotificationRepository notificationRepository,
                                  INotificationService notificationService,
                                  IScheduleRepository scheduleRepository
                                 )
        {
            _bookingRepository = bookingRepository;
            _quotationGRepository = quotationGRepository;
            _servicemanGRepository = servicemanGRepository;
            _employeeServices = employeeServices;
            _serviceGSchedule = serviceGSchedule;
            _serviceManRepository = serviceManRepository;
            _bookingGRepository = bookingGRepository;
            _notificationGRepository = notificationGRepository;
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
            _scheduleRepository = scheduleRepository;
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
        [Authorize(Policy = "SendQuotationPolicy")]
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
        [Authorize(Policy = "SendQuotationPolicy")]
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
                    await _notificationService.SendNotificationOfNewQuotation(booking.UserId, "Quotation", "New Quotation Send By Service Man");
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

        [Authorize(Policy = "ScheduleServicePolicy")]
        public async Task<IActionResult> ScheduleServices([FromBody] AllQuotationEmployeeViewModel model)
        {
            var quotation = await _quotationGRepository.GetByIdAsync(model.quotationId);
            ServiceSchedule ss = new ServiceSchedule
            {
                BookingId = quotation.BookingId,
                ServiceManId = quotation.ServiceManId,
                QuotationId = quotation.Id,
                ScheduledAt = DateTime.Now.AddDays(1),
                TotalPrice = quotation.Price,
                Status = "Scheduled"
            };

            quotation.Status = "Scheduled";
            await _quotationGRepository.SaveAsync();
            // save into serviceschedule 
            await _serviceGSchedule.InsertAsync(ss);
            await _serviceGSchedule.SaveAsync();
            // send notification 
            var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);
            await _notificationService.SendNotificationOfScheduleService(booking.UserId, "Schedule Service", $"Your Service Is Schedule At {ss.ScheduledAt}");
            return Ok();
        }
        public async Task<IActionResult> MyScheduleServices()
        {
            // Fatch Data From DB 
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await _employeeServices.GetAllScheduleServiceByEmployee(userId);
                return View(data);
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }
        public async Task<IActionResult> Notification()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var notifications = await _notificationRepository.GetAllNotificationByUserId(userId);
                return View(notifications);
            }
            catch (Exception ex) 
            {
                return View();
            }
            return View();
        }

        public async Task<IActionResult> IsSlotBooked(DateOnly date , TimeOnly time)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var schedules = await _scheduleRepository.GetAllScheduleSlotByServiceManId(userId);
            foreach (var schedule in schedules)
            {

                if(schedule.Date == date && schedule.Time == time)
                {
                    return Json($"This Slot Is Already Booked");
                }
                if(schedule.Date == date && schedule.Time == time.AddMinutes(119) || schedule.Time.AddMinutes(119) == time)
                {
                    return Json($"This Slot Is Conflict With Other Slot");
                }
            }
            return Json(true);
        }
    }
}
