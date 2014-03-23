#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics;

namespace EmailSenderProgram
{
    public static class EmailHandler
    {
        public static SmtpClient SmtpClient { get; private set; }
        public static MailTemplate EmailTemplate { get; private set; }

        static EmailHandler()
        {
            SmtpClient = new SmtpClient();
        }

        public static void SendMail(MailTemplate emailTemplate)
        {
            emailTemplate.validateBody();
            EmailTemplate = emailTemplate;
            DayOfWeek today = DateTime.Now.DayOfWeek;
            if (EmailTemplate.SendDays.IsNullOrEmpty() || EmailTemplate.SendDays.Contains(today))
            {
                Extensions.Echo("---------------Sending: " + EmailTemplate.GetType(), ConsoleColor.White);
                foreach (Customer customer in EmailTemplate.GetRecipients())
                {
                    using (MailMessage message = BuildEmail(customer.Email))
                    {
                        Extensions.Echo("To: " + customer.Email, ConsoleColor.Blue);
                        Extensions.Echo("Body: " + message.Body, ConsoleColor.DarkGreen);
                        //SmtpClient.Send(message);
                    }
                }
            }
        }

        private static MailMessage BuildEmail(string recipient)
        {
            MailMessage message = new MailMessage();
            message.From = EmailTemplate.From;
            message.Subject = EmailTemplate.Subject;
            message.Body = BuildBody(recipient);
            message.IsBodyHtml = EmailTemplate.IsBodyHtml;
            message.To.Add(recipient);
            return message;
        }

        private static string BuildBody(string recipient)
        {
            var swapPairs = new[] 
            { 
                new Tuple<string, object>("{email}", recipient),
                // add additional tuples for additional format items to replace
                // for example:
                // -------------------------------------------------------------------
                // new Tuple<string, object>("{name}", customername),
                // new Tuple<string, object>("{age}", customerage)
                // -------------------------------------------------------------------
                // where {name} and {age} are format items specified in BodyArguments
                // for the specific e-mail type, and customername and customerage
                // are properties for the specific customer
            };

            // take the contents of the BodyArguments property for the specified
            // EmailType, replace the custom format items (i.e. "{email}") using
            // the swapPairs array
            object[] bodyArguments = EmailTemplate.BodyArguments.Replace(swapPairs);

            // replace the actual format items in the body template with the formatted
            // body arguments, and return the body
            return string.Format(EmailTemplate.BodyTemplate, bodyArguments);
        }
    }
}
