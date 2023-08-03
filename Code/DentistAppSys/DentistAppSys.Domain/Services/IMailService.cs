using DentistAppSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistAppSys.Domain.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Appointment appointment);
    }
}
