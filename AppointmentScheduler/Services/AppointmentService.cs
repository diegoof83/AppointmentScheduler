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

        public async Task<int> Book(AppointmentVM model)
        {
            if (model == null)
                return 0;
            if (model.Id > 0)//update
            {
                return 1;
            }
            else//insert
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = GetEndTime(model),
                    ServiceProviderId = model.ServiceProviderId,
                    ClientId = model.ClientId,
                    ResponsableAdminId = model.ResponsableAdminId
                };

                _dbContext.Appointments.Add(appointment);
                await _dbContext.SaveChangesAsync();
                return 2;
            }
        }

        private DateTime GetEndTime(AppointmentVM model)
        {
            DateTime endtime = model.StartTime.AddMinutes(model.Duration);
            return endtime;
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
    }
}
