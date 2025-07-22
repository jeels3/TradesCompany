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
                return View("Error", new { message = ex.Message });
            }
        }
        public async Task<IActionResult> GetUserByServiceType(int serviceTypeId)
        {
            var service = await _userRepository.GetAllByServiceTypeServicemenAsync(serviceTypeId);
            return View(service);
        }

        [HttpGet]
        public async Task<IActionResult> BookService()
        {
            BookingViewModel model = new BookingViewModel
            {
                ServiceTypes = (List<ServiceType>)await _serviceGRepository.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookService(BookingViewModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                    return View(model);
                }
            }
            return RedirectToAction("BookService",model);
        }

        [HttpGet]
        public async Task<IActionResult> AllQuotation()
        {
            // Fatch By Booking -> User -> Quotation
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var quotations = await _quotationRepository.GetQuotationForUser(userId);
            return View(quotations);
        }

        //[HttpPost] // Customer Accept 
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

        [HttpGet]
        public async Task<IActionResult> MyScheduleServices()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var scheduleservices = await _employeeServices.GettAllScheduleServiceByUser(userId);
            return View(scheduleservices);
        }
    } 
}
