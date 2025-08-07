using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NuGet.Protocol;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    [Authorize(Roles = "USER")]
    public class UserController : Controller
    {
        private readonly IRepository<ServiceType> _serviceGRepository;
        private readonly IRepository<Quotation> _quotationGRepository;
        private readonly IRepository<Booking> _bookingGRepository;
        private readonly IRepository<ServiceMan> _serviceManGRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IHubContext<NotificationHub> _hubcontext;
        public readonly IEmployeeServices _employeeServices;
        private readonly INotificationService _notificationService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<ServiceSchedule> _serviceScheduleGRepository;

        public UserController(IRepository<ServiceType> serviceGReposuitory,
                              IUserRepository userRepository,
                              IRepository<Booking> bookingGRepository,
                              IRepository<Quotation> quotationGRepository,
                              IHubContext<NotificationHub> hubcontext,
                              IQuotationRepository quotationRepository,
                              IEmployeeServices employeeServices,
                              INotificationService notificationService,
                              IBookingRepository bookingRepository,
                              IRepository<ServiceMan> serviceManGRepository,
                              IRepository<ServiceSchedule> serviceScheduleGRepository
                              )
        {
            _serviceGRepository = serviceGReposuitory;
            _userRepository = userRepository;
            _bookingGRepository = bookingGRepository;
            _quotationGRepository = quotationGRepository;
            _hubcontext = hubcontext;
            _quotationRepository = quotationRepository;
            _employeeServices = employeeServices;
            _notificationService = notificationService;
            _bookingRepository = bookingRepository;
            _serviceManGRepository = serviceManGRepository;
            _serviceScheduleGRepository = serviceScheduleGRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var serviceTypeData = await _serviceGRepository.GetAllAsync();
                return View(serviceTypeData);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong During Access Dashboard");
                return View("Error", new { message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByServiceType(int serviceTypeId)
        {
            try
            {
                var service = await _userRepository.GetAllByServiceTypeServicemenAsync(serviceTypeId);
                return View(service);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> BookService()
        {
            try
            {
                BookingViewModel model = new BookingViewModel
                {
                    ServiceTypes = (List<ServiceType>)await _serviceGRepository.GetAllAsync()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> BookService(BookingViewModel model)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Booking booking = new Booking
                    {
                        ServiceTypeId = model.ServiceTypeId,
                        UserId = userId,
                        WorkDetails = model.WorkDetails,
                        imagepath = model?.imagepath,
                        Price = model.price
                    };

                    await _bookingGRepository.InsertAsync(booking);
                    await _bookingGRepository.SaveAsync();
                    await _notificationService.SendNotificationOfNewBooking(model.ServiceTypeId, "Booking", $"New Service Booked");
                    return RedirectToAction("MyBooking");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                    return View(model);
                }
            }
            return RedirectToAction("BookService", model);
        }

        public async Task<IActionResult> MyBooking()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var bookings = await _bookingRepository.GetAllBookingByUserId(userId);
                if (bookings == null || !bookings.Any())
                {
                    TempData["ErrorMessage"] = "No bookings found.";
                    return View(new List<Booking>());
                }
                return View(bookings);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> BookingCancel(int bookingId)
        {
            try
            {
                // Update Status : Booking , Quoation
                var booking = await _bookingGRepository.GetByIdAsync(bookingId);
                booking.Status = "Canceled";
                // If Any Quotation Available than update status to canceled
                var quotations = await _quotationRepository.GetQuotationByBookingId(bookingId);
                if (quotations != null && quotations.Any())
                {
                    foreach (var quotation in quotations)
                    {
                        quotation.Status = "Canceled";
                        var serviceMan = await _serviceManGRepository.GetByIdAsync(quotation.ServiceManId);
                        await _notificationService.SendNotificationOfNewQuotation(serviceMan.UserId, "Booking Cancel", "Booking Cancel On Which You Send Quotation");
                    }
                    await _quotationGRepository.SaveAsync();
                }
                await _bookingGRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Booking Canceled successfully." });
        }

        [HttpGet]
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> AllQuotation()
        {
            // Fatch By Booking -> User -> Quotation
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var quotations = await _quotationRepository.GetQuotationForUser(userId);
            if (quotations == null)
            {
                return View(new List<QuotationByUser>());
            }
            return View(quotations);
        }
        [HttpGet]
        public async Task<IActionResult> QuotationDetails(int quotationId)
        {
            try
            {
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                if (quotation == null)
                {
                    TempData["ErrorMessage"] = "Quotation not found.";
                    return BadRequest("Quotation Not Found");
                }

                decimal totalprice = quotation.Price;
                decimal platformfeesExcluded = totalprice - 100;
                decimal actualprice = platformfeesExcluded / (1 + 18m / 100m);

                var data = new
                {
                    quotationId = quotation.Id,
                    servicemancharge = actualprice,
                    gst = (actualprice * 18) / 100,
                    platformfees = 100,
                    totalprice = quotation.Price,
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return BadRequest("Something Went Wrong");
            }
        }
        //[HttpPost] // Customer Accept 
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> RequestAccept(int quotationId)
        {
            try
            {
                // Update Status : Booking , Quoation
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                var serviceMan = await _serviceManGRepository.GetByIdAsync(quotation.ServiceManId);
                quotation.Status = "Accepted";
                await _quotationGRepository.SaveAsync();
                // send notification
                await _notificationService.SendNotificationOfNewQuotation(serviceMan.UserId, "Quotation Accepted", "Your Quotation Is Accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Request Accept successfully." });
        }

        public async Task<IActionResult> RequestReject(int quotationId)
        {
            try
            {
                // Update Status : Booking , Quoation
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                var serviceMan = await _serviceManGRepository.GetByIdAsync(quotation.ServiceManId);
                quotation.Status = "Canceled";
                await _quotationGRepository.SaveAsync();
                // send notification
                await _notificationService.SendNotificationOfNewQuotation(serviceMan.UserId, "Quotation Rejected", "Your Quotation Is Rejected");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Request Reject successfully." });
        }

        [HttpGet]
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> MyScheduleServices()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var scheduleservices = await _employeeServices.GettAllScheduleServiceByUser(userId);
                return View(scheduleservices);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> ScheduleReject(int ScheduleServiceId)
        {
            try
            {
                // Update Status : Booking , Quoation
                var serviceSchedule = await _serviceScheduleGRepository.GetByIdAsync(ScheduleServiceId);
                var quotation = await _quotationGRepository.GetByIdAsync(serviceSchedule.QuotationId);
                var serviceMan = await _serviceManGRepository.GetByIdAsync(quotation.ServiceManId);
                quotation.Status = "Rejected";
                serviceSchedule.Status = "Rejected";
                await _serviceScheduleGRepository.SaveAsync();
                await _quotationGRepository.SaveAsync();
                // send notification
                await _notificationService.SendNotificationOfNewQuotation(serviceMan.UserId, "Schedule Rejected", "Your Schedule Is Rejected");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Request Reject successfully." });
        }
    }
}
