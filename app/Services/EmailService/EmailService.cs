using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Services.EmailService.Entity;
using MtdKey.OrderMaker.Services.EmailService.Templates;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using MtdKey.OrderMaker.AppConfig;
using Microsoft.Extensions.Options;
using System.Net;
using MtdKey.EmailBuilder;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;


namespace MtdKey.OrderMaker.Services.EmailService
{
    public class EmailService<TContext>(TContext context, 
        IOptions<EmailSettings> emailOptions, 
        ITemplateStorage templateStorage) : IEmailService where TContext : DbContext, IEmailServiceContext
    {
        private readonly TContext context = context;
        private readonly ITemplateStorage templateStorage = templateStorage;
        private readonly EmailSettings emailSettings = emailOptions.Value;

        public async Task AddEmailTaskAsync(string recepient, EmailTaskDesigner designer)
        {
            await context.EmailTasks.AddAsync(new EmailTask
            {
                Attributes = designer.GetAttributesAsJsonString(),
                Recepient = recepient,
                Subject = designer.Subject,
                Template = designer.Template,
            });

            await context.SaveChangesAsync();
        }

        public async Task<long[]> GetActiveTasksAsync()
        {
           return await context.EmailTasks
                .Where(x=>x.Complete == false)
                .Select(x=>x.Id)
                .ToArrayAsync();
        }

        public async Task SendEmailAsync(long taskId)
        {
            var task = await context.EmailTasks.FindAsync(taskId);
            if (task == null) return;

            var designer = EmailTaskTemplate.Items[task.Template];
            designer.MappingAttributes(task.Attributes);

            string body = await designer.MakeBodyMessageAsync(templateStorage, emailSettings);

            task.DeliveryStatus = Execute (task.Recepient, task.Subject, body);
            task.Complete = true;
            await context.SaveChangesAsync();
        }

        private bool Execute(string email, string subject, string message)
        {
            try
            {
                MailAddress toAddress = new(email);
                MailAddress fromAddress = new(emailSettings.FromAddress, emailSettings.FromName);         
                MailMessage mail = new(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                using SmtpClient smtp = new(emailSettings.SmtpServer, emailSettings.Port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSettings.FromAddress, emailSettings.Password),
                    EnableSsl = true
                };
                smtp.Send(mail);
            }
            catch
            {                
                return false;
            }


            return true;
        }
    }
}
