using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APBD5.Models
{
    public class Sequence
    {
        [Required(ErrorMessage = "IdProduct is required")]
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "IdWarehouse is required")]
        public int IdWarehouse { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime CreatedAt { get; set; }

    }
}
