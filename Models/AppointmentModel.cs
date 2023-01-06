using System;
using System.ComponentModel.DataAnnotations;

namespace gabinet_rejestracja.Models
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }

        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data wizyty")]
        public DateTime Date { get; set; }

        [Display(Name = "Komentarz")]
        public string? Comment { get; set; }
    }
}