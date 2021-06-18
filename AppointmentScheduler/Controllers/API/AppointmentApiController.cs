using AppointmentScheduler.Models.Responses;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentScheduler.Controllers.API
{
    [ApiController]
    [Route("api/Appointment")]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _loggedUserId;
        private readonly string _loggedUserRole;

        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            _loggedUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _loggedUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("Book")]
        public IActionResult Book(AppointmentVM appointment)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            try
            {
                response.Status = _appointmentService.Book(appointment).Result;
                if(response.Status == 1)
                {
                    response.Message = Helper.AppointmentUpdated;
                }
                if(response.Status == 2)
                {
                    response.Message = Helper.AppointmentAdded;
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }
    }
}
