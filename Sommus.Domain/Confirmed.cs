using System;
using System.ComponentModel.DataAnnotations;

namespace Sommus.Domain
{
    public class Confirmed
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Cases { get; set; }
    }
}
