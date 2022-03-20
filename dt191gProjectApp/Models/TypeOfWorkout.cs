using System.ComponentModel.DataAnnotations;
//dt191g projekt, Av Alice Fagerberg
namespace dt191gProjectApp.Models
{
    public class TypeOfWorkout
    {
        [Key]
        public int TypeId { get; set; }

        [Display(Name = "Konceptnamn")]
        [Required(ErrorMessage = "Du måste fylla i ett namn till konceptet")]
        public string? TypeName { get; set; }

        [Display(Name = "Konceptbeskrivning")]
        [Required(ErrorMessage = "Du måste fylla i en beskrvining")]
        [MaxLength(350)]
        public string? Description { get; set; }

        public List<Workout>? Workouts { get; set; }
    }
}
