using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MECA.ConsoleApp.Models
{
    public class ConsumptionData
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Consumption { get; set; }
    }
}
