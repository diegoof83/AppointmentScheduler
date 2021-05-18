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
    }
}
