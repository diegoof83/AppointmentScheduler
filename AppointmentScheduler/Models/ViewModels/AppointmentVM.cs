using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Models.ViewModels
{
    public class AppointmentVM
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        public string ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool IsApproved { get; set; }// The Service Provider is responsible for approvals
        public string ResponsableAdminId { get; set; } // Who has created the appointment        
        public string ResponsableAdminName { get; set; }
        public bool IsforClient { get; set; }// Flag that determines who is viewing the appointment(Client or provider).
                                             // Clients can not approve appointments.
    }
}
