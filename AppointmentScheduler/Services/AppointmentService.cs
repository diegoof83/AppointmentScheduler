using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
            if (model == null)
                return Helper.FailureStatus;
            if (model.Id > 0)//update
            {
                return Helper.UpdateStatus;
            }
            else//insert
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = DateTime.Parse(model.StartTime),
                    EndTime = GetEndTime(model),
                    Duration = model.Duration,
                    ServiceProviderId = model.ServiceProviderId,
                    ClientId = model.ClientId,
                    ResponsableAdminId = model.ResponsableAdminId,
                    IsApproved = false
                };

                _dbContext.Appointments.Add(appointment);
                await _dbContext.SaveChangesAsync();
                return Helper.SucessStatus;
            }
        }

        private DateTime GetEndTime(AppointmentVM model)
        {
            var endTime = DateTime.Parse(model.StartTime).AddMinutes(model.Duration);
            return endTime;
        }
        public List<AppointmentVM> GetServiceProviderAppointments(string providerId)
        {
            return _dbContext.Appointments.Where(x => x.ServiceProviderId == providerId)
                .Select(ap => new AppointmentVM()
                {
                    Id = ap.Id,
                    Description = ap.Description,
                    Title = ap.Title,
                    StartTime = ap.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = ap.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
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
                    StartTime = ap.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = ap.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Duration = ap.Duration,
                    IsApproved = ap.IsApproved
                }).ToList();
        }
    }
}
