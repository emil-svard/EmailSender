using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;

namespace EmailSenderProgram
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(100, 50);
            try
            {
                EmailHandler.SendMail(new MailTemplateWelcome());
                EmailHandler.SendMail(new MailTemplateComeBack());
                Extensions.Echo("\nAll mails have been sent!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Extensions.Echo(""+ex.ToString().Replace("\n", "\n\n"), ConsoleColor.Red);
            }
            Console.ReadKey();
        }
    }
}