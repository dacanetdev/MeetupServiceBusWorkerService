using DentistAppSys.API.Services;
using Microsoft.AspNetCore.Http;
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
    }
}
