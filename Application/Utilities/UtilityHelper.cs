using Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Models.Enums;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class UtilityHelper
    {
        /// <summary>
        /// Serialize request and response
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JsonSerializeRequestResponseLog(object data)
        {
            var stringdata = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            if (stringdata.Length > 10000)
                stringdata = "Json length more than 10,000";

            return stringdata;
        }

        /// <summary>
        /// Randomly generate string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijkmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generate a random password
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null!)
        {
            if (opts == null)
                opts = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public static T ParseEnum<T>(this string? value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static void MustNotHaveBeenSentToDepartment(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (ticketFromDb.Sent.HasValue && ticketFromDb.Sent.Value)
            {
                throw new CustomMessageException("Ticket has already been sent to a department");
            }
        }

        public static void CancelIfSentToDepartmentAndFinance(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if ((ticketFromDb.Sent.HasValue && ticketFromDb.Sent.Value) && (ticketFromDb.SentToFinance.HasValue && ticketFromDb.SentToFinance.Value))
            {
                throw new CustomMessageException("Ticket has already been sent to all departments");
            }
        }
        public static void MustHaveBeenSentToFinance(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (!ticketFromDb.SentToFinance.HasValue)
            {
                throw new CustomMessageException("Ticket must be sent to the finance department");
            }

            if (!ticketFromDb.SentToFinance.Value)
            {
                throw new CustomMessageException("Ticket must be sent to the finance department");
            }
        }

        public static void MustHvaeBeenSentToDepartment(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (!ticketFromDb.Sent.HasValue)
            {
                throw new CustomMessageException("Ticket must be sent to the department");
            }

            if (!ticketFromDb.Sent.Value)
            {
                throw new CustomMessageException("Ticket must be sent to the department");
            }
        }
    }
}
