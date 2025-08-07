using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Infrastructure.Services;
using TradesCompany.Web.ViewModel;

namespace TradesCompany.Web.Controllers
{
    [Authorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        private readonly IRepository<Quotation> _quotationGRepository;
        private readonly IRepository<ServiceMan> _servicemanGRepository;
        private readonly IRepository<ServiceSchedule> _serviceGSchedule;
        private readonly IRepository<Booking> _bookingGRepository;
        private readonly IRepository<Notification> _notificationGRepository;
        private readonly IRepository<Bill> _billGRepository;
        private readonly IRepository<ApplicationUser> _userGRepository;
        private readonly IRepository<ServiceSchedule> _serviceScheduleGRepository;
        private readonly IRepository<ServiceMan> _serviceManGRepository;

        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeServices _employeeServices;
        private readonly IServiceManRepository _serviceManRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly EmailService _emailService;
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
                                  IScheduleRepository scheduleRepository,
                                  EmailService emailService,
                                  IRepository<Bill> billGRepository,
                                  IRepository<ApplicationUser> userGRepository,
                                  IRepository<ServiceSchedule> serviceScheduleGRepository,
                                  IRepository<ServiceMan> serviceManGRepository
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
            _emailService = emailService;
            _billGRepository = billGRepository;
            _userGRepository = userGRepository;
            _serviceScheduleGRepository = serviceScheduleGRepository;
            _serviceManGRepository = serviceManGRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var servicemen = await _serviceManRepository.GetServiceManByUserId(userId);
            var booking = await _bookingRepository.GetAllBookingAsync(servicemen.ServiceTypeId);
            if (booking == null)
            {
                return View(new List<BookingByServiceType>());
            }
            return View(booking);
        }

        [HttpGet]
        [Authorize(Policy = "SendQuotationPolicy")]
        public async Task<IActionResult> CreateQuotation(int bookingId)
        {
            // Check if bookingId is valid
            if (bookingId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid booking ID.";
                return RedirectToAction("Dashboard");
            }
            var booking = await _bookingGRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Invalid booking ID. Please try again.";
                return RedirectToAction("Dashboard");
            }
            if (booking.Status.Equals("Completed") || booking.Status.Equals("Canceled"))
            {
                TempData["ErrorMessage"] = "booking Is Completed or Canceled";
                return RedirectToAction("Dashboard");
            }
            QuotationViewModel model = new QuotationViewModel
            {
                BookingId = bookingId,
            };
            return View(model);
        }

        public async Task<IActionResult> QuotationCancel(int quotationId)
        {
            try
            {
                // Update Status : Quoation
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);
                quotation.Status = "Rejected";
                await _quotationGRepository.SaveAsync();
                // send notification
                await _notificationService.SendNotificationOfNewQuotation(booking.UserId, "Quotation Rejected", "Your Booking on Quotation Is Rejected");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Request Reject successfully." });
        }

        [HttpPost]
        [Authorize(Policy = "SendQuotationPolicy")]
        public async Task<IActionResult> CreateQuotation(QuotationViewModel model)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var servicemen = await _serviceManRepository.GetServiceManByUserId(userId);
            var booking = await _bookingGRepository.GetByIdAsync(model.BookingId);

            if (ModelState.IsValid)
            {
                try
                {
                    // save DB
                    var withgst = model.Price + (model.Price * 18) / 100; // Assuming 18% GST
                    var amout = withgst + 100; // Assuming 100 is the platform fee

                    Quotation quotation = new Quotation
                    {
                        BookingId = model.BookingId,
                        ServiceManId = servicemen.Id,
                        Price = amout,
                        Status = "Pending",
                        QuotationPdf = "soon",
                        QuotationDescription = model.Description,
                    };

                    booking.Status = "Accepted";

                    await _quotationGRepository.InsertAsync(quotation);
                    await _quotationGRepository.SaveAsync();
                    await _bookingGRepository.SaveAsync();

                    await _notificationService.SendNotificationOfNewQuotation(booking.UserId, "Quotation", "New Quotation Send By Service Man");
                    return RedirectToAction("AllQuotation");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Something went wrong while creating the quotation. Please try again.";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllQuotation()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await _employeeServices.QuotationByServicerMan(userId);
            if (data == null)
            {
                return View(new List<QuotationByServicerMan>());
            }
            return View(data);
        }

        public async Task<IActionResult> GetQuotationDetails(int quotationId)
        {
            try
            {
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                if (quotation == null)
                {
                    TempData["ErrorMessage"] = "Quotation not found.";
                    return RedirectToAction("AllQuotation");
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
                    Description = quotation.QuotationDescription,
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while fetching quotation details. Please try again.";
                return BadRequest("Something Went Wrong");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditQuotation([FromForm] Dictionary<string, string> data)
        {
            var quotationId = int.Parse(data["quotationId"]);
            var price = decimal.Parse(data["price"]);
            var QuotationDescription = data["QuotationDescription"];
            try
            {
                var quotation = await _quotationGRepository.GetByIdAsync(quotationId);
                var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);
                if (booking == null)
                {
                    TempData["ErrorMessage"] = "Booking not found for the quotation.";
                    return BadRequest("Booking Not Found Of This Quotation");
                }
                if (quotation == null)
                {
                    TempData["ErrorMessage"] = "Quotation not found.";
                    return RedirectToAction("AllQuotation");
                }
                // Update quotation details
                var withgst = price + (price * 18) / 100; // Assuming 18% GST
                var amout = withgst + 100;
                quotation.Price = amout;
                quotation.QuotationDescription = QuotationDescription;


                quotation.Status = "Pending"; // Reset status to Pending after edit
                await _quotationGRepository.SaveAsync();
                await _notificationService.SendNotificationOfNewQuotation(booking.UserId, "Quotation", "Quotation Updated By Service Man");
                return Ok(new { message = "Quotation Update Succesfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while updating the quotation. Please try again.";
                return BadRequest("Something Went Wrong");
            }
        }

        [Authorize(Policy = "ScheduleServicePolicy")]
        [HttpPost]
        public async Task<IActionResult> ScheduleServices([FromForm] ScheduleDtos model)

        {
            try
            {
                // Parse the date and time
                if (!DateTime.TryParse(model.date, out DateTime scheduledDate) ||
                    !TimeSpan.TryParse(model.time, out TimeSpan scheduledTime))
                {
                    return Json(new { success = false, message = "Invalid date or time format." });
                }

                // Combine date and time
                DateTime requestedDateTime = scheduledDate.Date.Add(scheduledTime);

                // Validate that the requested time is in the future
                if (requestedDateTime <= DateTime.Now)
                {
                    return Json(new { success = false, message = "Please select a future date and time." });
                }

                var quotation = await _quotationGRepository.GetByIdAsync(model.quotationId);
                if (quotation == null)
                {
                    return Json(new { success = false, message = "Quotation not found." });
                }

                // Check for existing schedules within 2-hour window for this serviceman
                var existingSchedules = await _serviceGSchedule.GetAllAsync();
                var conflictingSchedule = existingSchedules
                    .Where(s => s.ServiceManId == quotation.ServiceManId &&
                               s.Status != "Cancelled" &&
                               s.Status != "Completed")
                    .Any(s => Math.Abs((s.ScheduledAt - requestedDateTime).TotalHours) < 2);

                if (conflictingSchedule)
                {
                    return Json(new
                    {
                        success = false,
                        message = "This time slot is not available. Please select a time at least 2 hours away from existing bookings."
                    });
                }

                // Create new service schedule
                ServiceSchedule ss = new ServiceSchedule
                {
                    BookingId = quotation.BookingId,
                    ServiceManId = quotation.ServiceManId,
                    QuotationId = quotation.Id,
                    ScheduledAt = requestedDateTime,
                    TotalPrice = quotation.Price,
                    Status = "Scheduled"
                };

                // Update quotation status
                quotation.Status = "Scheduled";
                await _quotationGRepository.SaveAsync();

                // Save service schedule
                await _serviceGSchedule.InsertAsync(ss);
                await _serviceGSchedule.SaveAsync();

                // Send notification
                var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);
                await _notificationService.SendNotificationOfScheduleService(
                    booking.UserId,
                    "Service Scheduled",
                    $"Your service has been scheduled for {ss.ScheduledAt:MMM dd, yyyy 'at' hh:mm tt}"
                );

                return Json(new
                {
                    success = true,
                    message = $"Service successfully scheduled for {ss.ScheduledAt:MMM dd, yyyy 'at' hh:mm tt}"
                });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new
                {
                    success = false,
                    message = "An error occurred while scheduling the service. Please try again."
                });
            }
        }

        [Authorize(Policy = "ScheduleServicePolicy")]
        [HttpGet]
        public async Task<IActionResult> MyScheduleServices()
        {
            // Fatch Data From DB 
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await _employeeServices.GetAllScheduleServiceByEmployee(userId);
                if (data == null)
                {
                    return View(new List<ScheduleServiceByEmployee>());
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while fetching your schedule services. Please try again.";
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
                var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);
                var serviceMan = await _serviceManGRepository.GetByIdAsync(quotation.ServiceManId);
                quotation.Status = "Rejected";
                serviceSchedule.Status = "Rejected";
                await _serviceScheduleGRepository.SaveAsync();
                await _quotationGRepository.SaveAsync();
                // send notification
                await _notificationService.SendNotificationOfNewQuotation(booking.UserId, "Schedule Rejected", "Your Booking Schedule Is Rejected");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { message = "Request Reject successfully." });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Notification()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var notifications = await _notificationRepository.GetAllNotificationByUserId(userId);
                if (notifications == null)
                {
                    return View(new List<Notification>());
                }
                return View(notifications);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while fetching notifications. Please try again.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCompleted(int ScheduleServiceId)
        {
            try
            {
                // Update Status : Booking , Quoation
                var serviceSchedule = await _serviceGSchedule.GetByIdAsync(ScheduleServiceId);
                var quotation = await _quotationGRepository.GetByIdAsync(serviceSchedule.QuotationId);
                var booking = await _bookingGRepository.GetByIdAsync(quotation.BookingId);

                serviceSchedule.Status = "Completed";
                quotation.Status = "Completed";
                booking.Status = "Completed";
                await _quotationGRepository.SaveAsync();
                var totalamount = serviceSchedule.TotalPrice;
                // Create Bill
                Bill model = new Bill
                {
                    Title = "Service Completed",
                    serviceCharge = totalamount - (totalamount * 18 / 100) - (totalamount * 5 / 100),
                    Gst = totalamount * 18 / 100, // Assuming 18% GST
                    PlatFormFees = totalamount * 5 / 100, // Assuming 5% Platform Fees
                    TotalPrice = totalamount,
                    serviceScheduleId = serviceSchedule.Id,

                };
                await _billGRepository.InsertAsync(model);
                await _billGRepository.SaveAsync();
                // Fatch serviceMan and customer 
                var serviceMan = await _servicemanGRepository.GetByIdAsync(quotation.ServiceManId);
                var servicemanuser = await _userGRepository.GetByIdAsync(serviceMan.UserId);
                var customer = await _userGRepository.GetByIdAsync(booking.UserId);
                InoviceViewModel invoice = new InoviceViewModel
                {
                    BillId = model.Id,
                    CustomerName = customer.UserName,
                    ServiceManName = servicemanuser.UserName,
                };
                // Create Bill PDF
                var html = $"<h1>hello , {invoice.CustomerName}</h1>" +
                    $"<h2>Service Completed Successfully</h2>" +
                    $"<p>Service Charge : {model.serviceCharge}</p>" +
                    $"<p>Playfor Charge : {model.PlatFormFees}</p>" +
                    $"<p>Gst : {model.Gst}</p>" +
                    $"<p>Total Amount : {model.TotalPrice}</p>" +
                    $"<p>ServiceMan : {invoice.ServiceManName}</p>" +
                    $"<p>ServiceMan : {servicemanuser.Email}</p>" +
                    $"<p>Contact Us : TradesCompany@gmail.com </p>"
                ;

                var renderer = new ChromePdfRenderer();

                var pdf = renderer.RenderHtmlAsPdf(html);
                byte[] pdfbytes = pdf.BinaryData;
                //pdf.SaveAs($"invoice-{invoice.BillId}.pdf"); for save pdf in local storage

                // send pdf throgh email
                var emailbody = "Please find the attached PDF document.";
                Attachment attachment = new Attachment(new MemoryStream(pdfbytes), $"invoice-{invoice.BillId}.pdf", MediaTypeNames.Application.Pdf);
                await _emailService.SendEmailAsync(attachment, customer.Email, "Service Completede", emailbody, false);
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong {ex.Message}");
            }
            return Ok(new { message = "Service Complete successfully." });
        }
    }
}
