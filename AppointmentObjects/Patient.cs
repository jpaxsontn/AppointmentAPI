using System.ComponentModel.DataAnnotations;

namespace AppointmentObjects
{
    public class Patient
    {
        public int Id { get; set; }
        [Required, MinLength(1)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
