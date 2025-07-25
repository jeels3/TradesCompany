using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using NuGet.Protocol;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Shared.Hubs;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IRepository<ServiceType> _serviceGRepository;
        private readonly IRepository<Quotation> _quotationGRepository;
        private readonly IRepository<Booking> _bookingGRepository;
        private readonly IUserRepository  _userRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IHubContext<NotificationHub> _hubcontext;
        public readonly  IEmployeeServices _employeeServices;
        private readonly INotificationService _notificationService;

        public UserController(IRepository<ServiceType> serviceGReposuitory,
                              IUserRepository userRepository,
                              IRepository<Booking> bookingGRepository,
                              IRepository<Quotation> quotationGRepository,
                              IHubContext<NotificationHub> hubcontext,
                              IQuotationRepository quotationRepository,
                              IEmployeeServices employeeServices,
                              INotificationService notificationService
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
        }           

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var serviceTypeData = await _serviceGRepository.GetAllAsync();
                return View(serviceTypeData);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong During Access Dashboard");
                return View("Error", new { message = ex.Message });
            }
        }
        public async Task<IActionResult> GetUserByServiceType(int serviceTypeId)
        {
            try
            {
                var service = await _userRepository.GetAllByServiceTypeServicemenAsync(serviceTypeId);
                return View(service);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        [Authorize(Policy ="BookingServicePolicy")]
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
        //[Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> BookService(BookingViewModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
                        imagepath = model?.imagepath
                    };

                    await _bookingGRepository.InsertAsync(booking);
                    await _bookingGRepository.SaveAsync();
                    await _notificationService.SendNotificationOfNewBooking(model.ServiceTypeId, "Booking", $"New Service Booked");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                    return View(model);
                }
            }
            return RedirectToAction("BookService",model);
        }

        [HttpGet]
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> AllQuotation()
        {
            // Fatch By Booking -> User -> Quotation
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var quotations = await _quotationRepository.GetQuotationForUser(userId);
            return View(quotations);
        }

        //[HttpPost] // Customer Accept 
        [Authorize(Policy = "BookingServicePolicy")]
        public async Task<IActionResult> RequestAccept(int quotationId)
        {
            try
            {
            // Update Status : Booking , Quoation
                var quotation =await _quotationGRepository.GetByIdAsync(quotationId);
                quotation.Status = "Accepted";
                _quotationGRepository.SaveAsync();
                // send notification            

            }catch(Exception ex)
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
                quotation.Status = "Rejected";
                _quotationGRepository.SaveAsync();
                // send notification            

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
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View();
            }
        }
    } 
}
