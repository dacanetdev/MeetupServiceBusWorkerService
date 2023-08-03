using System.ComponentModel.DataAnnotations;

namespace DentistAppSys.Domain.Models
{
    public class Appointment
    {
        [Required]
        public int AppointmentId { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public string? PatientName { get; set; }
        [Required]
        public string? PatientEmail { get; set; }
        [Required]
        public int CareTakerId { get; set; }
        [Required]
        public DateTime ScheduledAt { get; set; }
    }
}
