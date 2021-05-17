using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Services
{
    public class AppointmentService: IAppointmentService
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ClientVM> GetClientList()
        {
            throw new NotImplementedException();
        }

        public List<ServiceProviderVM> GetServiceProviderList()
        {
            throw new NotImplementedException();
        }
    }
}
