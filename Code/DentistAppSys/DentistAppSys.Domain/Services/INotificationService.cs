using DentistAppSys.Domain.Models;

namespace DentistAppSys.Domain.Services
{ 
    public interface INotificationService
    {
        public Task<long> ScheduleAppointmentNotificationAsync(Appointment appointment);
        public Task CancelAppointmentNotificationAsync(int id);
    }
}
