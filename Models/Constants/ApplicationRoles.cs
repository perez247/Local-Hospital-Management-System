using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Constants
{
    public class ApplicationRoles
    {
        /// <summary>
        /// Admin
        /// </summary>
        public const string Admin = "admin";

        /// <summary>
        /// nurse
        /// </summary>
        public const string Nurse = "nurse";

        /// <summary>
        /// pharmacy
        /// </summary>
        public const string Pharmacy = "pharmacy";

        /// <summary>
        /// surgery
        /// </summary>
        public const string Surgery = "surgery";

        /// <summary>
        /// lab
        /// </summary>
        public const string Lab = "lab";

        /// <summary>
        /// Doctor
        /// </summary>
        public const string Doctor = "doctor";

        /// <summary>
        /// Radiology
        /// </summary>
        public const string Radiology = "radiology";

        /// <summary>
        /// Admission
        /// </summary>
        public const string Admission = "admission";

        /// <summary>
        /// HR
        /// </summary>
        public const string HR = "hr";

        /// <summary>
        /// HR
        /// </summary>
        public const string Finance = "finance";

        /// <summary>
        /// staff
        /// </summary>
        public const string Staff = "staff";

        /// <summary>
        /// patient
        /// </summary>
        public const string Patients = "patient";

        /// <summary>
        /// Company
        /// </summary>
        public const string Company = "company";

        /// <summary>
        /// Reception
        /// </summary>
        public const string Reception = "reception";


        /// <summary>
        /// Other
        /// </summary>
        public const string Other = "other";

        /// <summary>
        /// All the roles in the
        /// </summary>
        /// <returns></returns>
        public static ICollection<string> Roles()
        {
            return new List<string>
            {
                Admin, 
                Nurse, 
                Pharmacy, 
                Surgery, 
                Lab, 
                Doctor, 
                Radiology, 
                Admission, 
                Patients, 
                Staff, 
                Company,
                HR,
                Finance,
                Reception,
                Other
            };
        }
    }

    public static class AppTax
    {
        public const decimal Basic = 0.01m;
    }
}
