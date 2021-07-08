﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Utility
{
    public static class Helper
    {
        //User Roles
        public static string Admin = "Admin";
        public static string Client = "Client";
        public static string ServiceProvider = "Tattoo Artist";

        //Session strings
        public static string LoggedUserSession = "ssLoggedUser";

        //String Formatations
        public static string DatetimeFormat = "yyyy-MM-dd HH:mm:ss";

        //Appointment Responses
        public static string AppointmentAdded = "Appointment added successfully.";
        public static string AppointmentUpdated = "Appointment updated successfully.";
        public static string AppointmentDeleted = "Appointment deleted successfully.";
        public static string AppointmentExists = "Appointment for selected date and time already exists.";
        public static string AppointmentConfirmed = "Appointment confirmed successfully.";
        public static string AppointmentNotExists = "Appointment does not exist.";
        public static string AppointmentAddError = "Something went wrong, Please try again.";
        public static string AppointmentUpdateError = "Something went wrong, Please try again.";
        public static string AppointmentConfirmError = "Something went wrong, Please try again.";
        public static string SomethingWentWrong = "Something went wrong, Please try again.";

        //Appointment Responses status
        public static int SucessStatus = 1;
        public static int FailureStatus = 0;
        public static int UpdateStatus = 2;

        //TempDatas
        public static string TempDataNewCreatedUserName = "tdUserName";
        
        //Email 
        public static string EmailAppointmentCreated = "Appointment Booked";
        public static string EmailFromName = "Appointment Booking";
        internal static object EmailFromAddress = "diegoof@gmail.com";

        public static string EmailMessageAppointmentCreated(string name, string dateBooked)
        {
            return $"Your appointment with {name} is booked on {dateBooked} and in pending status";
        }

        /// <summary>
        /// Create a list with roles of user types for drop-downs
        /// </summary>
        /// <returns>A list of all role options of users</returns>
        public static List<SelectListItem> UserRolesForDropDown(bool isAdmin)
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem{Text=Helper.ServiceProvider, Value=Helper.ServiceProvider},
                new SelectListItem{Text=Helper.Client, Value=Helper.Client}
            };

            if (isAdmin)
            {
                roles.Add(new SelectListItem { Text = Helper.Admin, Value = Helper.Admin });
            }

            return roles;
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
