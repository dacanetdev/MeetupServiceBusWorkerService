using Azure.Messaging.ServiceBus;
using DentistAppSys.API.Models;
using Newtonsoft.Json;

namespace DentistAppSys.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public NotificationService(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceBusClient = serviceBusClient;
            _serviceBusSender = _serviceBusClient.CreateSender(_configuration.GetValue<string>("QueueName"));
        }

        public async Task<long> ScheduleAppointmentNotificationAsync(Appointment appointment)
        {
            var messageContent = JsonConvert.SerializeObject(appointment);
            ServiceBusMessage serviceBusMessage = new(messageContent);
            var enqueueTime = appointment.ScheduleAt.AddHours(_configuration.GetValue<int>("EnqueueDifference"));

            var messageQueueNumber = await _serviceBusSender.ScheduleMessageAsync(serviceBusMessage, enqueueTime);

            return messageQueueNumber;
        }

        public async Task CancelAppointmentNotificationAsync(int messageQueueNumber)
        {
            await _serviceBusSender.CancelScheduledMessageAsync(messageQueueNumber);
        }
    }
}
