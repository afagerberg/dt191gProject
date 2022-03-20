using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//dt191g projekt, Av Alice Fagerberg
namespace dt191gProjectApp.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string? Member { get; set; }

        [ForeignKey("Workout")]
        [Required]
        public int WorkoutId { get; set; }
        public Workout? Workout { get; set; }
    }
}
