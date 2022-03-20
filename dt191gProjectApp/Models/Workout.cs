using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//dt191g projekt, Av Alice Fagerberg
namespace dt191gProjectApp.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        [Display(Name = "Instruktör")]
        [Required(ErrorMessage = "Du måste fylla i ett namn på instruktören")]
        public string? Trainer { get; set; }

        [Display(Name = "Veckodag")]
        [Required(ErrorMessage = "Du måste välja en veckodag")]
        [RegularExpression("Måndag|Tisdag|Onsdag|Torsdag|Fredag|Lördag|Söndag", ErrorMessage = "Du måste fylla i en dag när passet ska hållas")]
        public string? DayofWorkout { get; set; }

        [Display(Name = "Tid på dagen")]
        [Required(ErrorMessage = "Du måste fylla i en tidpunkt")]
        public string? Time { get; set; }

        [Display(Name = "Koncept")]
        [Required(ErrorMessage = "Passet måste ha ett koncept")]
        [ForeignKey("TypeOfWorkout")]
        public int TypeId { get; set; }
        public virtual TypeOfWorkout? TypeOfWorkout { get; set; }


        public List<Booking>? Bookings { get; set; }


    }
}
