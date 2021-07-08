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
        public IActionResult Book([FromBody] AppointmentVM appointment)
        {
            CommonResponse<int> response = new();
            try
            {
                response.Status = _appointmentService.Book(appointment).Result;
                if (response.Status == Helper.UpdateStatus)
                {
                    response.Message = Helper.AppointmentUpdated;
                }
                else if (response.Status == Helper.SucessStatus)
                {
                    response.Message = Helper.AppointmentAdded;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetAppointments")]
        public IActionResult GetAppointments(string providerId)
        {
            CommonResponse<List<AppointmentVM>> response = new();

            try
            {
                if (_loggedUserRole.Equals(Helper.Client))
                {
                    response.Model = _appointmentService.GetClientAppointments(_loggedUserId);
                }
                else if (_loggedUserRole.Equals(Helper.ServiceProvider))
                {
                    response.Model = _appointmentService.GetServiceProviderAppointments(_loggedUserId);
                }
                else//Admin
                {
                    response.Model = _appointmentService.GetServiceProviderAppointments(providerId);
                }
                response.Status = Helper.SucessStatus;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetAppointment/{id}")]
        public IActionResult GetAppointment(int id)
        {
            CommonResponse<AppointmentVM> response = new();

            try
            {
                response.Model = _appointmentService.GetAppointment(id);
                response.Status = Helper.SucessStatus;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            CommonResponse<int> response = new();

            try
            {
                response.Status = await _appointmentService.Delete(id);
                response.Message = response.Status == Helper.SucessStatus ? Helper.AppointmentDeleted : Helper.SomethingWentWrong;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Confirm/{id}")]
        public async Task<IActionResult> Confirm(int id)
        {
            CommonResponse<int> response = new();

            try
            {
                response.Status = await _appointmentService.ConfirmBoking(id);
                response.Message = response.Status == Helper.SucessStatus ? Helper.AppointmentConfirmed : Helper.AppointmentConfirmError;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = Helper.FailureStatus;
            }

            return Ok(response);
        }
    }
}
