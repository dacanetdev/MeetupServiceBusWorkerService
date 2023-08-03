using DentistAppSys.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace DentistAppSys.Domain.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(Appointment appointment)
        {
            MailMessage mail = new();
            mail.To.Add(addresses: appointment?.PatientEmail ?? string.Empty);
            mail.Subject = $"You have an appointment scheduled at {appointment?.ScheduledAt.ToString("g")}";
            mail.Body = $@"<p>Hello there {appointment.PatientName}!
              <br /><br />You have a dental appointment scheduled at {appointment.ScheduledAt.ToString("g")}.
              <br /><br />Thanks
              <br />Your Dentist :)</p>
            ";
            mail.IsBodyHtml = true;

            var fromAddress = _configuration.GetSection("EmailId").Value;
            mail.From = new MailAddress(fromAddress);

            using var smtpClient = new SmtpClient();
            smtpClient.Host = _configuration.GetSection("SMTPHost").Value;
            smtpClient.Port = Convert.ToInt32(_configuration.GetSection("SMTPPort").Value);
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(fromAddress, _configuration.GetSection("AppPassword").Value);

            try
            {
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
