using System;
using System.ComponentModel.DataAnnotations;

namespace Sommus.Domain
{
    public class Deaths
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Cases { get; set; }
    }
}
