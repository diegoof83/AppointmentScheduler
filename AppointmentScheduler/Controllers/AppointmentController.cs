using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.ClientList = _appointmentService.GetClientList();
            ViewBag.ServiceProviderList = _appointmentService.GetServiceProviderList();
            ViewBag.Duration = Helper.GetTimeDropDown();
            return View();
        }
    }
}
