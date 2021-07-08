using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        public AppointmentService(ApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Get the list of users that are clients registered in the application
        /// </summary>
        /// <returns>ClienteVM List</returns>
        public List<ClientVM> GetClientList()
        {
            //using projection = projecting the users to clientVM
            var clients = (from users in _dbContext.Users
                           join userRoles in _dbContext.UserRoles on users.Id equals userRoles.UserId
                           join roles in _dbContext.Roles.Where(x => x.Name.Equals(Helper.Client)) on userRoles.RoleId equals roles.Id
                           orderby users.FirstName, users.LastName
                           select new ClientVM
                           {
                               Id = users.Id,
                               Name = users.FullName
                           }).ToList();

            return clients;
        }

        /// <summary>
        /// Get the list of users that are service providers registered in the application
        /// </summary>
        /// <returns>ServiceProviderVM List</returns>
        public List<ServiceProviderVM> GetServiceProviderList()
        {
            var serviceProvider = (from users in _dbContext.Users
                                   join userRoles in _dbContext.UserRoles on users.Id equals userRoles.UserId
                                   join roles in _dbContext.Roles.Where(x => x.Name.Equals(Helper.ServiceProvider)) on userRoles.RoleId equals roles.Id
                                   orderby users.FirstName, users.LastName
                                   select new ServiceProviderVM
                                   {
                                       Id = users.Id,
                                       Name = users.FullName
                                   }).ToList();

            return serviceProvider;
        }

        public async Task<int> Book(AppointmentVM model)
        {
            if (model is null)
                return Helper.FailureStatus;

            if (model.Id > 0)//update
            {
                var appointment = _dbContext.Appointments.FirstOrDefault(ap => ap.Id == model.Id);                
                if (appointment is null)
                    return Helper.FailureStatus;

                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartTime = DateTime.Parse(model.StartTime);
                appointment.EndTime = GetEndTime(model);
                appointment.Duration = model.Duration;
                appointment.ClientId = model.ClientId;
                appointment.ResponsableAdminId = model.ResponsableAdminId;//TODO - Get the Admin ID and set as responsible for the appointment                 

                await _dbContext.SaveChangesAsync();
                //TODO - Send Email after Updates
                return Helper.UpdateStatus;
            }
            else//insert
            {
                Appointment appointment = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = DateTime.Parse(model.StartTime),
                    EndTime = GetEndTime(model),
                    Duration = model.Duration,
                    ServiceProviderId = model.ServiceProviderId,
                    ClientId = model.ClientId,
                    ResponsableAdminId = model.ResponsableAdminId,//TODO - Get the Admin ID and set as responsible for the appointment 
                    IsApproved = false
                };

                _dbContext.Appointments.Add(appointment);
                await _dbContext.SaveChangesAsync();
                SendEmail(appointment);
                return Helper.SucessStatus;
            }
        }

        private static DateTime GetEndTime(AppointmentVM model)
        {
            var endTime = DateTime.Parse(model.StartTime).AddMinutes(model.Duration);
            return endTime;
        }

        private async void SendEmail(Appointment appointment)
        {
            var client = _dbContext.Users.FirstOrDefault(u => u.Id == appointment.ClientId);
            var serviceProvider = _dbContext.Users.FirstOrDefault(u => u.Id == appointment.ServiceProviderId);
            var dateBooked = appointment.StartTime.ToString(Helper.DatetimeFormat);

            //send email to service provider
            await _emailSender.SendEmailAsync(serviceProvider.Email, Helper.EmailAppointmentCreated, Helper.EmailMessageAppointmentCreated(client.FullName, dateBooked));
            //send email to client
            await _emailSender.SendEmailAsync(client.Email, Helper.EmailAppointmentCreated, Helper.EmailMessageAppointmentCreated(serviceProvider.FullName, dateBooked));
        }

        public List<AppointmentVM> GetServiceProviderAppointments(string providerId)
        {
            return _dbContext.Appointments.Where(x => x.ServiceProviderId == providerId)
                .Select(ap => new AppointmentVM()
                {
                    Id = ap.Id,
                    Description = ap.Description,
                    Title = ap.Title,
                    StartTime = ap.StartTime.ToString(Helper.DatetimeFormat),
                    EndTime = ap.EndTime.ToString(Helper.DatetimeFormat),
                    Duration = ap.Duration,
                    IsApproved = ap.IsApproved
                }).ToList();
        }

        public List<AppointmentVM> GetClientAppointments(string clientId)
        {
            return _dbContext.Appointments.Where(x => x.ClientId == clientId)
                .Select(ap => new AppointmentVM()
                {
                    Id = ap.Id,
                    Description = ap.Description,
                    Title = ap.Title,
                    StartTime = ap.StartTime.ToString(Helper.DatetimeFormat),
                    EndTime = ap.EndTime.ToString(Helper.DatetimeFormat),
                    Duration = ap.Duration,
                    IsApproved = ap.IsApproved
                }).ToList();
        }

        public AppointmentVM GetAppointment(int id)
        {
            return _dbContext.Appointments.Where(ap => ap.Id == id)
                .Select(ap => new AppointmentVM()
                {
                    Id = ap.Id,
                    Description = ap.Description,
                    Title = ap.Title,
                    StartTime = ap.StartTime.ToString(Helper.DatetimeFormat),
                    EndTime = ap.EndTime.ToString(Helper.DatetimeFormat),
                    Duration = ap.Duration,
                    IsApproved = ap.IsApproved,
                    ClientId = ap.ClientId,
                    ServiceProviderId = ap.ServiceProviderId,
                    ClientName = _dbContext.Users.Where(u => u.Id == ap.ClientId).Select(u => u.FullName).FirstOrDefault(),
                    ServiceProviderName = _dbContext.Users.Where(u => u.Id == ap.ServiceProviderId).Select(u => u.FullName).FirstOrDefault()
                }).SingleOrDefault();
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(ap => ap.Id == id);

            if (appointment is not null)
            {
                _dbContext.Appointments.Remove(appointment);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> ConfirmBoking(int id)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(ap => ap.Id == id);

            if (appointment is not null)
            {
                appointment.IsApproved = true;
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
