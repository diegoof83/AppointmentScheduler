using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Client = "Client";
        public static string ServiceProvider = "Tattoo Artist";

        /// <summary>
        /// Create a list with roles of user types for dropdowns
        /// </summary>
        /// <returns>A list of all role options of users</returns>
        public static List<SelectListItem> UserRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Text=Helper.Admin, Value=Helper.Admin},
                new SelectListItem{Text=Helper.ServiceProvider, Value=Helper.ServiceProvider},
                new SelectListItem{Text=Helper.Client, Value=Helper.Client}
            };
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 7; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr" });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " Hr 30 min" });
                minute = minute + 30;
            }
            return duration;
        }
    }
}
