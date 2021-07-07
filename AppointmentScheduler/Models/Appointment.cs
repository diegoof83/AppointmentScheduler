using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public string ServiceProviderId { get; set; }
        public string ClientId { get; set; }
        public bool IsApproved { get; set; }// The Service Provider is responsible for approvals
        public string ResponsableAdminId { get; set; } // Who has created the appointment
    }
}
