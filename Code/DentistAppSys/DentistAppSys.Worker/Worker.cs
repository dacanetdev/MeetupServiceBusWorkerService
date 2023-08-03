using Azure.Messaging.ServiceBus;
using DentistAppSys.Domain.Models;
using DentistAppSys.Domain.Services;
using Newtonsoft.Json;

namespace DentistAppSys.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;

        public Worker(ServiceBusClient serviceBusClient, IMailService mailService, IConfiguration configuration, ILogger<Worker> logger)
        {
            _serviceBusClient = serviceBusClient;
            _mailService = mailService;
            _configuration = configuration;
            _logger = logger;

            _serviceBusReceiver = _serviceBusClient.CreateReceiver(_configuration.GetValue<string>("QueueName"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"FullyQualifiedNameSpace is {_serviceBusReceiver.FullyQualifiedNamespace}");

                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync();
                    while(message != null)
                    {
                        _logger.LogInformation(message.Body.ToString());
                        Appointment? data = JsonConvert.DeserializeObject<Appointment>(message.Body.ToString());

                        if(data == null)
                        {
                            _logger.LogError("Message content was null");
                        }

                        if(string.IsNullOrWhiteSpace(data.PatientEmail))
                        {
                            _logger.LogError("Patient Email doesn't exist in the payload");
                        }

                        await _mailService.SendEmailAsync(data);
                        await _serviceBusReceiver.CompleteMessageAsync(message);
                        message = await _serviceBusReceiver.ReceiveMessageAsync();
                    }
                    _logger.LogInformation("Waiting for 2 minutes now");
                    await Task.Delay(120000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}