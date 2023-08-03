
using Azure.Messaging.ServiceBus;
using DentistAppSys.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DentistAppSys.Domain.Services
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
            _serviceBusSender = _serviceBusClient.CreateSender(_configuration.GetSection("QueueName").Value);
        }

        public async Task<long> ScheduleAppointmentNotificationAsync(Appointment appointment)
        {
            var messageContent = JsonConvert.SerializeObject(appointment);
            ServiceBusMessage serviceBusMessage = new(messageContent);
            var enqueueTime = appointment.ScheduledAt.AddMinutes(Convert.ToInt32(_configuration.GetSection("EnqueueDifference").Value));

            var messageQueueNumber = await _serviceBusSender.ScheduleMessageAsync(serviceBusMessage, enqueueTime);

            return messageQueueNumber;
        }

        public async Task CancelAppointmentNotificationAsync(int messageQueueNumber)
        {
            await _serviceBusSender.CancelScheduledMessageAsync(messageQueueNumber);
        }
    }
}
