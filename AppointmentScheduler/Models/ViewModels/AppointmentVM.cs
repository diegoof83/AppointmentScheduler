using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Models.ViewModels
{
    public class AppointmentVM : Appointment
    {
        public new int? Id { get; set; }
        public string ServiceProviderName { get; set; }
        public string ClientName { get; set; }
        public string ResponsableAdminName { get; set; }
        public bool IsforClient { get; set; }// Flag where it can be determine who is viewing the appointment. Clients can not approve appointments
    }
}
