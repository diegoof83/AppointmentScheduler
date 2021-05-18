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
