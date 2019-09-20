using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using MimeKit;

namespace Chest
{
    public class EmailService
    {

        public void SendEmail(string email, string subject, string message)
        {
            Assembly senderAssembly = Assembly.LoadFrom("C:/Users/user/source/repos/Chest/SenderLibrary/bin/Debug/netcoreapp2.1/SenderLibrary.dll");

            Type emailSenderType = senderAssembly.GetType("SenderLibrary.EmailSender", true, true);

            object obj = Activator.CreateInstance(emailSenderType);
            MethodInfo method = emailSenderType.GetMethod("Send");
            method?.Invoke(obj, new object[]{email, subject, message});
        }

    }
}
