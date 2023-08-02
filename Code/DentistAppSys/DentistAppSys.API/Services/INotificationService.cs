using DentistAppSys.API.Models;

namespace DentistAppSys.API.Services
{
    public interface INotificationService
    {
        public Task<long> ScheduleAppointmentNotificationAsync(Appointment appointment);
        public Task CancelAppointmentNotificationAsync(int id);
    }
}
