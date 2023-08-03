using DentistAppSys.Domain.Models;
using DentistAppSys.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentistAppSys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public AppointmentController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                var messageQueueNumber = await _notificationService.ScheduleAppointmentNotificationAsync(appointment);

                return new OkObjectResult(new { MessageQueueNumber = messageQueueNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("cancel/{messageQueueNumber:long}")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int messageQueueNumber)
        {
            try
            {
                if(messageQueueNumber < 0)
                {
                    return new BadRequestObjectResult("Invalid value for Message Queue Number");
                }

                await _notificationService.CancelAppointmentNotificationAsync(messageQueueNumber);

                return new OkObjectResult("Appointment cancelled successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
