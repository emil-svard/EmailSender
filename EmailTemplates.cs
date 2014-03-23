using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics;

namespace EmailSenderProgram
{
    public abstract class MailTemplate : MailMessage
    {
        public string BodyTemplate { get; protected set; }
        public object[] BodyArguments { get; protected set; }
        public DayOfWeek[] SendDays { get; protected set; }

        public MailTemplate()
        {
            IsBodyHtml = true;
        }

        public abstract IEnumerable<Customer> GetRecipients();

        [Conditional("DEBUG")]
        public void validateBody()
        {
            if (string.IsNullOrWhiteSpace(BodyTemplate) || BodyArguments == null || string.IsNullOrWhiteSpace(Subject))
                throw new Exception("One or more of BodyTemplate, BodyArguments and Subject is null or empty in " + this);
            if (BodyTemplate.ExpectedArguments() != BodyArguments.Length)
                throw new Exception("Number of BodyArguments does not match number of format items in BodyTemplate in " + this);
        }
    }

    public class MailTemplateWelcome : MailTemplate
    {
        public MailTemplateWelcome()
        {
            From = new MailAddress("info@cdon.com");
            Subject = "Welcome as a new customer at CDON!";
            BodyTemplate = "Hi {0}<br />We would like to welcome you as customer on our site!<br /><br />" +
                           "Best Regards,<br />CDON Team";
            BodyArguments = new object[] { "{email}" };
        }
        public override IEnumerable<Customer> GetRecipients() { return DataFactory.GetNewCustomers(); }
    }

    public class MailTemplateComeBack : MailTemplate
    {
        public MailTemplateComeBack()
        {
            From = new MailAddress("infor@cdon.com");
            Subject = "We miss you as a customer";
            BodyTemplate = "Hi {0}<br />We miss you as a customer. Our shop is filled with nice products. " +
                           "Here is a voucher that gives you 50 kr to shop for.<br />Voucher: {1}<br /><br />" +
                           "Best Regards,<br />CDON Team";
            BodyArguments = new object[] { "{email}", Guid.NewGuid() };
            SendDays = new[] { DayOfWeek.Sunday };
        }
        public override IEnumerable<Customer> GetRecipients() { return DataFactory.GetCustomersWithNoOrders(); }
    }
}
